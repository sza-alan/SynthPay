namespace SynthPay.Transactions.Infrastructure.Outbox
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty; // Nome do Evento (ex: TransactionCreated)
        public string Content { get; set; } = string.Empty; // O JSON com os dados do evento
        public DateTime OccurredOnUtc { get; set; }
        public DateTime? ProcessedOnUtc { get; set; } // Se for null, significa que ainda não foi enviado
        public string? Error { get; set; }
    }
}
