using Core.Application.ServiceLifeTimes;
using Core.Domain.IRepositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Core.Infrastructure.Repositories.UnitOfWork;

public abstract class CleanBaseUnitOfWork<TContext, TCommand, TQuery> : ICleanBaseScoped
    where TContext : DbContext
    where TCommand : ICleanBaseCommandUnitOfWork
    where TQuery : ICleanBaseQueryUnitOfWork
{
    private readonly TContext _context;
    protected CleanBaseUnitOfWork(TContext context)
    {
        _context = context;

        Commands = CreateInstance<TCommand>();
        Queries = CreateInstance<TQuery>();  
    }
    public TCommand Commands { get; protected set; }
    public TQuery Queries { get; protected set; }

    public DbContext GetDbContext()
    {
        return _context;
    }

    public virtual async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    #region Private
    private TType CreateInstance<TType>()
    {
        ConstructorInfo commandConstructor = typeof(TType).GetConstructor(new Type[] { typeof(TContext) })!;
        return (TType)commandConstructor!.Invoke(new object[] { _context });
    }
    #endregion
}
