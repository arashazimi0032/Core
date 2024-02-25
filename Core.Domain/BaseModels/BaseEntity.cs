using Core.Domain.BaseEvents;
using Core.Domain.Primitives;

namespace Core.Domain.BaseModels;

public abstract class BaseEntity<TId> : Entity, IEquatable<BaseEntity<TId>>
    where TId : notnull
{
    public TId Id { get; protected set; }
    public bool Equals(BaseEntity<TId>? other)
    {
        return Equals((object?)other);
    }
    public override bool Equals(object? obj)
    {
        return obj is BaseEntity<TId> entity && Id.Equals(entity.Id);
    }
    public static bool operator ==(BaseEntity<TId> left, BaseEntity<TId> right)
    {
        return Equals(left, right);
    }
    public static bool operator !=(BaseEntity<TId> left, BaseEntity<TId> right)
    {
        return !Equals(left, right);
    }
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
