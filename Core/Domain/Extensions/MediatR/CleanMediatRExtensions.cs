using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Domain.Extensions.MediatR;

internal static class CleanMediatRExtensions
{
    internal static IServiceCollection AddCleanMediatR(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);
        });
        
        return services;
    }
}
