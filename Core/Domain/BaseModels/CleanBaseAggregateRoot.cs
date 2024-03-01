namespace Core.Domain.BaseModels;

public abstract class CleanBaseAggregateRoot<TId> : CleanBaseEntity<TId>
    where TId : CleanValueObject
{
}
