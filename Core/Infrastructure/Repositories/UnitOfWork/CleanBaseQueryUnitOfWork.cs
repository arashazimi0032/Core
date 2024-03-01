using Core.Domain.IRepositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Repositories.UnitOfWork;

public abstract class CleanBaseQueryUnitOfWork<TContext> : ICleanBaseQueryUnitOfWork
    where TContext : DbContext
{
    private readonly TContext _context;
    protected CleanBaseQueryUnitOfWork(TContext context)
    {
        _context = context;
    }
}
