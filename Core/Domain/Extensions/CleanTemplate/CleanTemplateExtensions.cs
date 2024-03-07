using Core.Domain.Extensions.LifeTime;
using Core.Domain.Extensions.MediatR;
using Core.Domain.Extensions.Middleware;
using Core.Domain.Extensions.MinimalApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Domain.Extensions.CleanTemplate;

public static class CleanTemplateExtensions
{
    public static IServiceCollection AddCleanTemplate(this IServiceCollection services)
    {
        var assembly = Assembly.GetCallingAssembly();

        services.AddCleanMediatR(assembly);
        services.AddCleanMinimalApi();

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

    public static IEndpointRouteBuilder MapCleanTemplate(this IEndpointRouteBuilder builder)
    {
        builder.MapCleanMinimalApi();

        return builder;
    }
}
