using MassTransit;
using Microsoft.EntityFrameworkCore;
using SynthPay.Contracts.Events;
using SynthPay.Ledger.Worker.Domain.Entities;
using SynthPay.Ledger.Worker.Infrastructure.Persistence;

namespace SynthPay.Ledger.Worker.Consumers
{
    public class TransactionCreatedEventConsumer : IConsumer<TransactionCreatedEvent>
    {
        private readonly ILogger<TransactionCreatedEventConsumer> _logger;
        private readonly LedgerDbContext _dbContext;

        public TransactionCreatedEventConsumer(ILogger<TransactionCreatedEventConsumer> logger, LedgerDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<TransactionCreatedEvent> context)
        {
            var evento = context.Message;

            _logger.LogInformation("Processando crédito de R$ {Amount} para a conta {AccountId}...", evento.Amount, evento.AccountId);

            var balance = await _dbContext.Balances.FirstOrDefaultAsync(b => b.AccountId == evento.AccountId);

            if (balance == null)
            {
                balance = new AccountBalance(evento.AccountId);
                await _dbContext.Balances.AddAsync(balance);
            }

            balance.Credit(evento.Amount);

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Saldo atualizado com sucesso! Novo Saldo: R$ {CurrentBalance}", balance.CurrentBalance);
        }
    }
}
