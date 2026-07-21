namespace Ordering.Domain.Abstractions;

public interface IAggregate<T> : IEntity<T>, IAggregate { }

public interface IAggregate : IEntity
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    IDomainEvent[] ClearDomainEvents();
}

public abstract class Aggregate<TId> : Entity<TId>, IAggregate<TId>
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public IDomainEvent[] ClearDomainEvents()
    {
        IDomainEvent[] dequeuedEvents = [.. _domainEvents];
        _domainEvents.Clear();

        return dequeuedEvents;
    }
}
