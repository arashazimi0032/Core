using Core.Domain.IRepositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Repositories.UnitOfWork;

public abstract class CleanBaseUnitOfWork<TContext, TCommand, TQuery>
    where TContext : DbContext
    where TCommand : ICleanBaseCommandUnitOfWork
    where TQuery : ICleanBaseQueryUnitOfWork
{
    private readonly TContext _context;
    protected CleanBaseUnitOfWork(TContext context)
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
