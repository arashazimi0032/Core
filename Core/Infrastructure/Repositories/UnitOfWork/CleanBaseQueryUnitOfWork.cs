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
        CreateInstance();
    }

    private void CreateInstance()
    {
        foreach (var property in GetType().GetProperties())
        {
            var value = typeof(TContext).Assembly
                .GetTypes()
                .Where(t =>
                t.IsAssignableTo(property.PropertyType) &&
                t is { IsInterface: false, IsAbstract: false, IsClass: true })
                .FirstOrDefault()!;
            
            property.SetValue(this, Activator.CreateInstance(value, _context));
        }
    }
}
