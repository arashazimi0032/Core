using Core.Domain.BaseModels;
using Core.Domain.IRepositories.Queries;
using Core.Domain.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Infrastructure.Repositories.Queries;

public class CleanBaseQueryRepositry<TContext, TEntity, TId> : ICleanBaseQueryRepository<TEntity, TId>
    where TContext : DbContext
    where TEntity : CleanBaseEntity<TId>
    where TId : notnull
{
    private readonly TContext _context;
    protected DbSet<TEntity> dbSet;
    protected IQueryable<TEntity> dbSetAsNoTrack;

    protected CleanBaseQueryRepositry(TContext context)
    {
        _context = context;
        dbSet = _context.Set<TEntity>();
        dbSetAsNoTrack = _context.Set<TEntity>().AsNoTracking();
    }

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await dbSet.FindAsync(id, cancellationToken);

        if (entity is null) return null;

        _context.Entry(entity).State = EntityState.Detached;
        return entity;
    }
    public TEntity? GetById(TId id)
    {
        var entity = dbSet.Find(id);

        if (entity is null) return null;

        _context.Entry(entity).State = EntityState.Detached;
        return entity;
    }
    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await dbSetAsNoTrack.FirstOrDefaultAsync(predicate);
    }
    public TEntity? Get(Expression<Func<TEntity, bool>> predicate)
    {
        return dbSetAsNoTrack.FirstOrDefault(predicate);
    }
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbSetAsNoTrack.ToListAsync(cancellationToken);
    }
    public IEnumerable<TEntity> GetAll()
    {
        return dbSetAsNoTrack.ToList();
    }
    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await dbSetAsNoTrack.Where(predicate).ToListAsync(cancellationToken);
    }
    public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
    {
        return dbSetAsNoTrack.Where(predicate).ToList();
    }
    public async Task<CleanPaginatedList<TEntity>> GetPaginatedAsync(CleanPage page, CancellationToken cancellationToken = default)
    {
        return await dbSetAsNoTrack.ToPaginatedListAsync(page, cancellationToken);
    }
    public CleanPaginatedList<TEntity> GetPaginated(CleanPage page)
    {
        return dbSetAsNoTrack.ToPaginatedList(page);
    }
    public async Task<CleanPaginatedList<TEntity>> GetPaginatedAsync(Expression<Func<TEntity, bool>> predicate, CleanPage page, CancellationToken cancellationToken = default)
    {
        return await dbSetAsNoTrack.Where(predicate).ToPaginatedListAsync(page, cancellationToken);
    }
    public CleanPaginatedList<TEntity> GetPaginated(Expression<Func<TEntity, bool>> predicate, CleanPage page)
    {
        return dbSetAsNoTrack.Where(predicate).ToPaginatedList(page);
    }
    public IQueryable<TEntity> GetQueryable()
    {
        return dbSet;
    }
    public IQueryable<TEntity> GetQueryableAsNoTrack()
    {
        return dbSetAsNoTrack;
    }
}
