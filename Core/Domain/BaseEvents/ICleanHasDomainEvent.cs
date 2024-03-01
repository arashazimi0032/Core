namespace Core.Domain.BaseEvents;

public interface ICleanHasDomainEvent
{
    public IReadOnlyList<ICleanBaseDomainEvent> DomainEvents { get; }
    public void ClearDomainEvents();
    void Raise(ICleanBaseDomainEvent domainEvent);
}
