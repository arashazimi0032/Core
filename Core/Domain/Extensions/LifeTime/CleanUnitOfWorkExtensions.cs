using Core.Infrastructure.Repositories.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Domain.Extensions.LifeTime;

internal static class CleanUnitOfWorkExtensions
{
    internal static IServiceCollection AddCleanUnitOfWork(this IServiceCollection services, IEnumerable<Type> types)
    {
        IEnumerable<Type> unitOfWorkTypes = types.Where(t =>
                                                        t.BaseType != null &&
                                                        t.BaseType.IsGenericType &&
                                                        t.BaseType.GetGenericTypeDefinition() == typeof(CleanBaseUnitOfWork<,,>));

        foreach (var type in unitOfWorkTypes)
        {
            services.AddScoped(type);
        }

        return services;
    }
}
