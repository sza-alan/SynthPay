namespace SynthPay.Transactions.Domain.Enums
{
    public enum TransactionStatus
    {
        Pending = 1, // Toda transação nasce pendente
        Completed = 2,
        Failed = 3
    }
}
