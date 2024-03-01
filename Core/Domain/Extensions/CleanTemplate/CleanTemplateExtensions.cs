using Core.Domain.Extensions.LifeTime;
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
}
