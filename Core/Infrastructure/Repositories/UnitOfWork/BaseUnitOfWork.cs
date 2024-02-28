using Core.Domain.IRepositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Repositories.UnitOfWork;

public abstract class BaseUnitOfWork<TContext, TCommand, TQuery>
    where TContext : DbContext
    where TCommand : IBaseCommandUnitOfWork
    where TQuery : IBaseQueryUnitOfWork
{
    private readonly TContext _context;
    protected BaseUnitOfWork(TContext context)
    {
        _context = context;
        Commands = CreateCommandInstance(context);
        Queries = CreateQueryInstance(context);
    }
    public TCommand Commands { get; protected set; }
    public TQuery Queries { get; protected set; }
    protected abstract TCommand CreateCommandInstance(TContext context);
    protected abstract TQuery CreateQueryInstance(TContext context);

    public DbContext GetDbContext()
    {
        return _context;
    }

    public virtual async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}
