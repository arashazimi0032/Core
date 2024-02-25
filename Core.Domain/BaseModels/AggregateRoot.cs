namespace Core.Domain.BaseModels;

public class AggregateRoot<TId> : BaseStrongEntity<TId>
    where TId : ValueObject
{
}
