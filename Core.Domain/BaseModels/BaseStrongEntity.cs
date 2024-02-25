namespace Core.Domain.BaseModels;

public abstract class BaseStrongEntity<TId> : Entity<TId>
    where TId : ValueObject
{
}
