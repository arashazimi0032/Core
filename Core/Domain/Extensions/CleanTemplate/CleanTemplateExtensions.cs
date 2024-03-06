using Core.Domain.Extensions.LifeTime;
using Core.Domain.Extensions.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Domain.Extensions.CleanTemplate;

public static class CleanTemplateExtensions
{
    public static IServiceCollection AddCleanTemplate(this IServiceCollection services)
    {
        var assembly = Assembly.GetCallingAssembly();

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(assembly);
        });

        services
            .AddLifeTimeServices(assembly);

        return services;
    }

    public static IApplicationBuilder UseCleanTemplate(this IApplicationBuilder app)
    {
        var assembly = Assembly.GetCallingAssembly();

        app.UseCleanMiddlewares(assembly);

        return app;
    }
}
