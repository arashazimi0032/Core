using Core.Domain.BaseModels;
using Core.Domain.Pagination;
using System.Linq.Expressions;

namespace Core.Domain.IRepositories.Queries;

public interface IBaseQueryRepository<TEntity, TId>
    where TEntity : BaseEntity<TId>
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
    Task<PaginatedList<TEntity>> GetPaginatedAsync(Page page, CancellationToken cancellationToken = default);
    PaginatedList<TEntity> GetPaginated(Page page);
    Task<PaginatedList<TEntity>> GetPaginatedAsync(Expression<Func<TEntity, bool>> predicate, Page page, CancellationToken cancellationToken = default);
    PaginatedList<TEntity> GetPaginated(Expression<Func<TEntity, bool>> predicate, Page page);
    IQueryable<TEntity> GetQueryable();
    IQueryable<TEntity> GetQueryableAsNoTrack();
}
