using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SynthPay.Contracts.Events;
using SynthPay.Transactions.Infrastructure.Persistence;
using System.Text.Json;

namespace SynthPay.Transactions.Infrastructure.BackgroundJobs
{
    public class OutboxProcessorBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OutboxProcessorBackgroundService> _logger;

        public OutboxProcessorBackgroundService(IServiceProvider serviceProvider, ILogger<OutboxProcessorBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("🚀 Outbox Processor Worker iniciado!");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessOutboxMessages(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagens do Outbox.");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private async Task ProcessOutboxMessages(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<SynthPayDbContext>();

            var bus = scope.ServiceProvider.GetRequiredService<IBus>();

            var messages = await dbContext.OutboxMessages
                .Where(m => m.ProcessedOnUtc == null)
                .Take(20)
                .ToListAsync(stoppingToken);

            if (!messages.Any()) return;

            foreach ( var message in messages)
            {
                try
                {
                    var domainEvent = JsonSerializer.Deserialize<TransactionCreatedEvent>(message.Content);

                    if (domainEvent != null)
                    {
                        await bus.Publish(domainEvent, stoppingToken);
                    }

                    message.ProcessedOnUtc = DateTime.UtcNow;
                    _logger.LogInformation("Mensagem {MessageId} publicada com sucesso no RabbitMQ!", message.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao publicar a mensagem {MessageId}", message.Id);
                    message.Error = ex.Message;
                }

                await dbContext.SaveChangesAsync(stoppingToken);
            }
        }
    }
}
