namespace Core.Domain.BaseModels;

public abstract class CleanBaseEntity<TId> : CleanEntity, IEquatable<CleanBaseEntity<TId>>
    where TId : notnull
{
    public TId Id { get; protected set; }
    public bool Equals(CleanBaseEntity<TId>? other)
    {
        return Equals((object?)other);
    }
    public override bool Equals(object? obj)
    {
        return obj is CleanBaseEntity<TId> entity && Id.Equals(entity.Id);
    }
    public static bool operator ==(CleanBaseEntity<TId> left, CleanBaseEntity<TId> right)
    {
        return Equals(left, right);
    }
    public static bool operator !=(CleanBaseEntity<TId> left, CleanBaseEntity<TId> right)
    {
        return !Equals(left, right);
    }
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
