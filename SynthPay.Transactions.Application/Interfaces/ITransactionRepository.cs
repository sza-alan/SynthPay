using SynthPay.Transactions.Domain.Entities;

namespace SynthPay.Transactions.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction, CancellationToken cancellationToken);
    }
}