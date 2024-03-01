using Core.Domain.IRepositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Repositories.UnitOfWork;

public abstract class CleanBaseCommandUnitOfWork<TContext> : ICleanBaseCommandUnitOfWork
    where TContext : DbContext
{
    private readonly TContext _context;
    protected CleanBaseCommandUnitOfWork(TContext context)
    {
        _context = context;
    }
}
