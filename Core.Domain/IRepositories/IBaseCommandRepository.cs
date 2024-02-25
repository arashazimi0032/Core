using Core.Domain.BaseModels;

namespace Core.Domain.IRepositories;

public interface IBaseCommandRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : notnull
{
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(params TEntity[] entities);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    TEntity Update(TEntity entity);
    void UpdateRange(params TEntity[] entity);
    void UpdateRange(IEnumerable<TEntity> entity);
    TEntity Remove(TEntity entity);
    void RemoveRange(params TEntity[] entity);
    void RemoveRange(IEnumerable<TEntity> entity);
}
