using Core.Domain.BaseModels;
using Core.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Repositories.Commands;

public abstract class BaseCommandRepository<TContext, TEntity, TId> : IBaseCommandRepository<TEntity, TId>
    where TContext : DbContext
    where TEntity : Entity<TId>
    where TId : notnull
{
    private readonly TContext _context;
    protected DbSet<TEntity> dbSet;

    protected BaseCommandRepository(TContext context)
    {
        _context = context;
        dbSet = _context.Set<TEntity>();
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return (await dbSet.AddAsync(entity, cancellationToken)).Entity;
    }

    public async Task AddRangeAsync(params TEntity[] entities)
    {
        await dbSet.AddRangeAsync(entities);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public TEntity Remove(TEntity entity)
    {
        return dbSet.Remove(entity).Entity;
    }

    public void RemoveRange(params TEntity[] entity)
    {
        dbSet.RemoveRange(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entity)
    {
        dbSet.RemoveRange(entity);
    }

    public TEntity Update(TEntity entity)
    {
        return dbSet.Update(entity).Entity;
    }

    public void UpdateRange(params TEntity[] entity)
    {
        dbSet.UpdateRange(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entity)
    {
        dbSet.UpdateRange(entity);
    }
}
