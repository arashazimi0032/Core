using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Domain.Extensions.MinimalApi;

internal static class CleanMinimalApiExtensions
{
    internal static IServiceCollection AddCleanMinimalApi(this IServiceCollection services)
    {
        services.AddCarter();
        return services;
    }

    internal static IEndpointRouteBuilder MapCleanMinimalApi(this IEndpointRouteBuilder builder)
    {
        builder.MapCarter();
        return builder;
    }
}
