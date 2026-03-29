using MassTransit;
using SynthPay.Contracts.Events;

namespace SynthPay.Ledger.Worker.Consumers
{
    public class TransactionCreatedEventConsumer : IConsumer<TransactionCreatedEvent>
    {
        private readonly ILogger<TransactionCreatedEventConsumer> _logger;

        public TransactionCreatedEventConsumer(ILogger<TransactionCreatedEventConsumer> logger) => _logger = logger;

        public Task Consume(ConsumeContext<TransactionCreatedEvent> context)
        {
            var evento = context.Message;

            _logger.LogInformation(
                "\n💰 [LEDGER SERVICE] EVENTO RECEBIDO VIA RABBITMQ!\n" +
                "   Transação: {TransactionId}\n" +
                "   Conta: {AccountId}\n" +
                "   Valor a creditar: R$ {Amount}\n",
            evento.TransactionId, evento.AccountId, evento.Amount);

            return Task.CompletedTask;
        }
    }
}
