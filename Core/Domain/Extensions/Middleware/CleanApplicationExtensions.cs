using Core.Domain.Extensions.Middleware;
using Core.Presentation.Middleware;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace Core.Domain.Extensions.Middleware;

internal static class CleanApplicationExtensions
{
    internal static IApplicationBuilder UseCleanMiddlewares(this IApplicationBuilder app, Assembly assembly)
    {
        app.UseBaseMiddleware(assembly);

        return app;
    }

    private static IApplicationBuilder UseBaseMiddleware(this IApplicationBuilder app, Assembly assembly)
    {
        IEnumerable<Type> types = assembly
            .GetTypes()
            .Where(t =>
            t.IsAssignableTo(typeof(ICleanBaseMiddleware)) &&
            t is { IsInterface: false, IsAbstract: false, IsClass: true });

        foreach (var type in types)
        {
            app.UseMiddleware(type);
        }

        return app;
    }
}
