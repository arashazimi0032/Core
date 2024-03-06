using Core.Domain.BaseEvents;
using Core.Domain.Primitives;

namespace Core.Domain.BaseModels;

public abstract class CleanEntity : ICleanBaseHasDomainEvent, ICleanAuditable
{
    private readonly List<ICleanBaseDomainEvent> _domainEvents = new();
    public IReadOnlyList<ICleanBaseDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    public void Raise(ICleanBaseDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
