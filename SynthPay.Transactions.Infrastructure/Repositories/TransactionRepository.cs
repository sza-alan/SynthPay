using SynthPay.Transactions.Application.Interfaces;
using SynthPay.Transactions.Domain.Entities;
using SynthPay.Transactions.Infrastructure.Outbox;
using SynthPay.Transactions.Infrastructure.Persistence;
using System.Text.Json;

namespace SynthPay.Transactions.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly SynthPayDbContext _context;

        public TransactionRepository(SynthPayDbContext context) => _context = context;

        public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken)
        {
            await _context.Transactions.AddAsync(transaction, cancellationToken);

            var evento = new
            {
                TransactionId = transaction.Id,
                transaction.AccountId,
                transaction.Amount
            };

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                Type = "TransactionCreatedEvent",
                Content = JsonSerializer.Serialize(evento) // Guardamos o evento em JSON!
            };

            await _context.OutboxMessages.AddAsync(outboxMessage, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
