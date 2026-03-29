namespace SynthPay.Ledger.Worker.Domain.Entities
{
    public class AccountBalance
    {
        public Guid AccountId { get; private set; }
        public decimal CurrentBalance { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }

        private AccountBalance() { }

        public AccountBalance(Guid accountId)
        {
            AccountId = accountId;
            CurrentBalance = 0;
            LastUpdatedAt = DateTime.UtcNow;
        }

        public void Credit(decimal amount)
        {
            if (amount < 0) 
                throw new ArgumentException("O valor de crédito deve ser maior que zero.");

            CurrentBalance += amount;
            LastUpdatedAt = DateTime.UtcNow;
        }
    }
}
