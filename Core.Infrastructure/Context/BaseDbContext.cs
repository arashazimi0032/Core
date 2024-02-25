using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Context;

public abstract class BaseDbContext<TContext> : DbContext
    where TContext : DbContext
{
    protected BaseDbContext(DbContextOptions<TContext> options) : base(options)
    {
    }
}
