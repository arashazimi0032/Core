using Core.Presentation.Endpoint;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace Core.Domain.Extensions.Endpoint;

internal static class CleanEndpointExtensions
{
    internal static IApplicationBuilder UseCleanEndpoints(this IApplicationBuilder builder, Assembly assembly)
    {
        IEnumerable<Type> types = assembly
            .GetTypes()
            .Where(t => 
            t.IsAssignableTo(typeof(ICleanBaseEndpoint)) && 
            t is { IsInterface: false, IsAbstract: false, IsClass: true});

        builder.UseEndpoints(e =>
        {
            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                if (instance is null) continue;

                var endpoint = (ICleanBaseEndpoint)instance;
                
                endpoint.AddRoutes(e);
            }
        });

        return builder;
    }
}
