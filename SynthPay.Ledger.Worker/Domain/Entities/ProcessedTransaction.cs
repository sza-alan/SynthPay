namespace SynthPay.Ledger.Worker.Domain.Entities
{
    public class ProcessedTransaction
    {
        public Guid TransactionId { get; private set; }
        public DateTime ProcessedAt { get; private set; }

        private ProcessedTransaction() { }

        public ProcessedTransaction(Guid transactionId)
        {
            TransactionId = transactionId;
            ProcessedAt = DateTime.UtcNow;
        }
    }
}
