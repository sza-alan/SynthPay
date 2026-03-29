namespace SynthPay.Contracts.Events
{
    public record TransactionCreatedEvent(Guid TransactionId, Guid AccountId, decimal Amount);
}