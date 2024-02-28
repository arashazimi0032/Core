using Core.Domain.IRepositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Repositories.UnitOfWork;

public abstract class BaseCommandUnitOfWork<TContext> : IBaseCommandUnitOfWork
    where TContext : DbContext
{
    private readonly TContext _context;
    protected BaseCommandUnitOfWork(TContext context)
    {
        _context = context;
    }
}
