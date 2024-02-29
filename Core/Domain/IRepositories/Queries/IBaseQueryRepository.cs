using Core.Domain.BaseModels;

namespace Core.Domain.IRepositories.Queries;

public interface IBaseQueryRepository<TEntity, TId>
    where TEntity : BaseEntity<TId>
    where TId : notnull
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    TEntity? GetById(TId id);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    IEnumerable<TEntity> GetAll();
    Task<IQueryable<TEntity>> GetQueryableAsync(CancellationToken cancellationToken = default);
    Task<IQueryable<TEntity>> GetQueryableAsNoTrackAsync(CancellationToken cancellationToken = default);
}
