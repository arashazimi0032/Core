using Core.Domain.BaseModels;
using Core.Domain.IRepositories.Queries;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Repositories.Queries;

public class BaseQueryRepositry<TContext, TEntity, TId> : IBaseQueryRepository<TEntity, TId>
    where TContext : DbContext
    where TEntity : BaseEntity<TId>
    where TId : notnull
{
    private readonly TContext _context;
    protected DbSet<TEntity> dbSet;
    protected IQueryable<TEntity> dbSetAsNoTrack;

    protected BaseQueryRepositry(TContext context)
    {
        _context = context;
        dbSet = _context.Set<TEntity>();
        dbSetAsNoTrack = _context.Set<TEntity>().AsNoTracking();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbSetAsNoTrack.ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await dbSet.FindAsync(id, cancellationToken);

        if (entity is null) return null;

        _context.Entry(entity).State = EntityState.Detached;
        return entity;
    }

    public async Task<IQueryable<TEntity>> GetQueryableAsNoTrackAsync(CancellationToken cancellationToken = default)
    {
        return dbSetAsNoTrack;
    }

    public async Task<IQueryable<TEntity>> GetQueryableAsync(CancellationToken cancellationToken = default)
    {
        return dbSet;
    }
}
