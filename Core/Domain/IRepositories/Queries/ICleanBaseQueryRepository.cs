using Core.Domain.BaseModels;
using Core.Domain.Pagination;
using System.Linq.Expressions;

namespace Core.Domain.IRepositories.Queries;

public interface ICleanBaseQueryRepository<TEntity, TId>
    where TEntity : CleanBaseEntity<TId>
    where TId : notnull
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    TEntity? GetById(TId id);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    TEntity? Get(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    IEnumerable<TEntity> GetAll();
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
    Task<CleanPaginatedList<TEntity>> GetPaginatedAsync(CleanPage page, CancellationToken cancellationToken = default);
    CleanPaginatedList<TEntity> GetPaginated(CleanPage page);
    Task<CleanPaginatedList<TEntity>> GetPaginatedAsync(Expression<Func<TEntity, bool>> predicate, CleanPage page, CancellationToken cancellationToken = default);
    CleanPaginatedList<TEntity> GetPaginated(Expression<Func<TEntity, bool>> predicate, CleanPage page);
    IQueryable<TEntity> GetQueryable();
    IQueryable<TEntity> GetQueryableAsNoTrack();
}
