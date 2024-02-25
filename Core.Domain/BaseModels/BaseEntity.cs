namespace Core.Domain.BaseModels;

public abstract class BaseEntity<TId> : Entity<TId>
    where TId : struct
{
}
