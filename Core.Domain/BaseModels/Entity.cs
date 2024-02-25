using Core.Domain.BaseEvents;
using Core.Domain.Primitives;

namespace Core.Domain.BaseModels;

public class Entity : IHasDomainEvent, IAuditable
{
    private readonly List<IBaseDomainEvent> _domainEvents = new();
    public IReadOnlyList<IBaseDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    public void Raise(IBaseDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
