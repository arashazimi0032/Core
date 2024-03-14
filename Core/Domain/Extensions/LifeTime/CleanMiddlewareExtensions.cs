using Core.Presentation.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Domain.Extensions.LifeTime;

internal static class CleanMiddlewareExtensions
{
    internal static IServiceCollection AddCleanMiddlewares(this IServiceCollection services, IEnumerable<Type> types)
    {
        IEnumerable<Type> middlewares = types.Where(t => t.IsAssignableTo(typeof(ICleanBaseMiddleware)));

        foreach (Type middleware in middlewares)
        {
            services.AddTransient(middleware);
        }

        return services;
    }
}
