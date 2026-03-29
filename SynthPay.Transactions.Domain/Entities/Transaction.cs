using SynthPay.Transactions.Domain.Enums;

namespace SynthPay.Transactions.Domain.Entities
{
    public sealed class Transaction : Entity
    {
        public Guid AccountId { get; private set; }
        public decimal Amount { get; private set; }
        public TransactionType Type { get; private set; }
        public TransactionStatus Status { get; private set; }

        private Transaction() { }

        public Transaction(Guid accountId, decimal amount, TransactionType type)
        {
            if (amount <= 0)
                throw new ArgumentException("O valor da transação deve ser maior que zero.");

            AccountId = accountId;
            Amount = amount;
            Type = type;
            Status = TransactionStatus.Pending;
        }

        public void MarkAsCompleted()
        {
            Status = TransactionStatus.Completed;
        }

        public void MarkAsFailed()
        {
            Status = TransactionStatus.Failed;
        }
    }
}
