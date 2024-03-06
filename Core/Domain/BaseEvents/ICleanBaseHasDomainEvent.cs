using Core.Application.ServiceLifeTimes;

namespace Core.Domain.BaseEvents;

public interface ICleanBaseHasDomainEvent : ICleanBaseIgnore
{
    public IReadOnlyList<ICleanBaseDomainEvent> DomainEvents { get; }
    public void ClearDomainEvents();
    void Raise(ICleanBaseDomainEvent domainEvent);
}
