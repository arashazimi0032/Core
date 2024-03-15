using Core.Domain.DIModules;
using Core.Domain.Extensions.Endpoint;
using Core.Domain.Extensions.GlobalExceptionHandling;
using Core.Domain.Extensions.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Domain.Extensions.CleanTemplate;

public static class CleanTemplateExtensions
{
    public static IServiceCollection AddCleanTemplateDIModule<TDIModule>(this IServiceCollection services)
        where TDIModule : CleanBaseDIModule
    {
        var assembly = typeof(TDIModule).Assembly;

        services.AddCleanTemplate(assembly);

        return services;
    }
    public static IServiceCollection AddCleanTemplate(this IServiceCollection services)
    {
        var assembly = Assembly.GetCallingAssembly();

        services.AddCleanTemplate(assembly);

        return services;
    }

    public static IApplicationBuilder UseCleanTemplate(this IApplicationBuilder app)
    {
        var assembly = Assembly.GetCallingAssembly();

        app.UseCleanGlobalExceptionHandling();

        app.UseRouting();

        app.UseCleanMiddlewares(assembly);

        app.UseCleanEndpoints(assembly);

        return app;
    }
}
