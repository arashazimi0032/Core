using Core.Domain.Primitives;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Context;

public abstract class BaseIdentityDbContext<TContext, TUser> : IdentityDbContext<TUser>
    where TContext : IdentityDbContext<TUser>
    where TUser : IdentityUser
{
    protected BaseIdentityDbContext(DbContextOptions<TContext> options) : base(options)
    {
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<IAuditable>();

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
