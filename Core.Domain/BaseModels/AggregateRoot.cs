namespace Core.Domain.BaseModels;

public class AggregateRoot<TId> : BaseEntity<TId>
    where TId : ValueObject
{
}
