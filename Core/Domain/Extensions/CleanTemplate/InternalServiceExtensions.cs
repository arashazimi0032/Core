using Core.Domain.Extensions.LifeTime;
using Core.Domain.Extensions.MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Domain.Extensions.CleanTemplate;

internal static class InternalServiceExtensions
{
    internal static IServiceCollection AddCleanTemplate(this IServiceCollection services, Assembly assembly)
    {
        services.AddCleanMediatR(assembly);

        services.AddLifeTimeServices(assembly);

        return services;
    }
}
