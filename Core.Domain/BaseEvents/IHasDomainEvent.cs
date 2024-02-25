namespace Core.Domain.BaseEvents;

public interface IHasDomainEvent
{
    public IReadOnlyList<IBaseDomainEvent> DomainEvents { get; }
    public void ClearDomainEvents();
    void Raise(IBaseDomainEvent domainEvent);
}
