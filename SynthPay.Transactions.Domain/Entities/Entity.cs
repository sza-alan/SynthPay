namespace SynthPay.Transactions.Domain.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        private readonly List<object> _domainEvents = new();

        public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

        protected Entity() 
        { 
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        protected void AddDomainEvent(object domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        protected void ClearDomainEvent()
        {
            _domainEvents.Clear();
        }
    }
}
