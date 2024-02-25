using Core.Domain.IRepositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Repositories.UnitOfWork;

public abstract class BaseQueryUnitOfWork<TContext> : IBaseQueryUnitOfWork
    where TContext : DbContext
{
    private readonly TContext _context;
    protected BaseQueryUnitOfWork(TContext context)
    {
        _context = context;
    }
}
