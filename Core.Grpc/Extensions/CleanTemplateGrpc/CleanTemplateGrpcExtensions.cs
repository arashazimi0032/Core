using Core.Grpc.BaseContracts;
using Core.Grpc.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Grpc.Extensions.CleanTemplateGrpc;

public static class CleanTemplateGrpcExtensions
{
    public static IServiceCollection AddCleanGrpc(this IServiceCollection services)
    {
        services.AddServiceModelGrpc();

        services.AddSingleton<ICleanGrpcClientFactory, CleanGrpcClientFactory>();

        return services;
    }

    public static void MapCleanGrpc(this IEndpointRouteBuilder builder, Assembly assembly)
    {
        var types = assembly
            .GetTypes()
            .Where(t =>
            t.IsAssignableTo(typeof(ICleanBaseGrpcContract)) &&
            t is { IsInterface: false, IsAbstract: false, IsClass: true });

        MethodInfo registerMethod = typeof(GrpcEndpointRouteBuilderExtensions).GetMethod("MapGrpcService", BindingFlags.Public | BindingFlags.Static)!;

        foreach (var type in types)
        {
            MethodInfo genericMethod = registerMethod.MakeGenericMethod(type);

            genericMethod.Invoke(null, new object[] { builder });
        }
    }
}
