using Core.Application.ServiceLifeTimes;
using Core.Domain.BaseModels;

namespace Core.Domain.IRepositories.Commands;

public interface ICleanBaseCommandRepository<TEntity> : ICleanBaseIgnore
    where TEntity : CleanEntity
{
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(params TEntity[] entities);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    TEntity Add(TEntity entity);
    void AddRange(params TEntity[] entities);
    void AddRange(IEnumerable<TEntity> entities);
    TEntity Update(TEntity entity);
    void UpdateRange(params TEntity[] entity);
    void UpdateRange(IEnumerable<TEntity> entity);
    TEntity Remove(TEntity entity);
    void RemoveRange(params TEntity[] entity);
    void RemoveRange(IEnumerable<TEntity> entity);
}
