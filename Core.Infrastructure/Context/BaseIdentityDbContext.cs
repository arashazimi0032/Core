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
}
