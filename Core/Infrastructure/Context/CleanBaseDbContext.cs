using Core.Application.ServiceLifeTimes;
using Core.Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Context;

public abstract class CleanBaseDbContext<TContext> : DbContext, ICleanBaseIgnore
    where TContext : DbContext
{
    protected CleanBaseDbContext(DbContextOptions<TContext> options) : base(options)
    {
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<ICleanAuditable>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(e => e.CreatedAt).CurrentValue = DateTime.Now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property(e => e.ModifiedAt).CurrentValue = DateTime.Now;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
