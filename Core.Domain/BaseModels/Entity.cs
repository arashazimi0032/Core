using Core.Domain.BaseEvents;
using Core.Domain.Primitives;

namespace Core.Domain.BaseModels;

public abstract class Entity<TId> : IEquatable<Entity<TId>>, IHasDomainEvent, IAuditable
    where TId : notnull
{
    public TId Id { get; protected set; }
    private readonly List<IBaseDomainEvent> _domainEvents = new();
    public IReadOnlyList<IBaseDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    protected void Raise(IBaseDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public bool Equals(Entity<TId>? other)
    {
        return Equals((object?)other);
    }
    public override bool Equals(object? obj)
    {
        return obj is Entity<TId> entity && Id.Equals(entity.Id);
    }
    public static bool operator ==(Entity<TId> left, Entity<TId> right)
    {
        return Equals(left, right);
    }
    public static bool operator !=(Entity<TId> left, Entity<TId> right)
    {
        return !Equals(left, right);
    }
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
