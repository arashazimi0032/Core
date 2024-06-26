﻿using Core.Application.ServiceLifeTimes;
using Core.Domain.Exceptions;
using Core.Domain.Extensions.Settings;
using Core.Presentation.Middleware;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections;
using System.Reflection;

namespace Core.Domain.Extensions.LifeTime;

internal static class CleanLifeTimeExtensions
{
    internal static IServiceCollection AddLifeTimeServices(this IServiceCollection services, Assembly assembly)
    {
        IEnumerable<Type> types = assembly
            .GetTypes()
            .GetWithInterfaceClasses();

        services
            .AddTransientServices(types)
            .AddSingletonServices(types)
            .AddScopedServices(types);

        return services;
    }
    private static IServiceCollection AddTransientServices(this IServiceCollection services, IEnumerable<Type> types)
    {
        types = types.Where(t => t.IsAssignableTo(typeof(ICleanBaseTransient)));

        foreach (Type type in types)
        {
            IEnumerable<Type> interfaces = GetImplementedInterfaces(type);
            foreach (Type interfaceType in interfaces)
            {
                services.AddTransient(interfaceType, type);
            }
        }

        services.AddCleanMiddlewares(types);

        return services;
    }
    private static IServiceCollection AddSingletonServices(this IServiceCollection services, IEnumerable<Type> types)
    {
        types = types.Where(t => t.IsAssignableTo(typeof(ICleanBaseSingleton)));

        foreach (Type type in types)
        {
            IEnumerable<Type> interfaces = GetImplementedInterfaces(type);
            foreach (Type interfaceType in interfaces)
            {
                services.AddSingleton(interfaceType, type);
            }
        }

        services.AddCleanSettings(types);

        return services;
    }
    private static IServiceCollection AddScopedServices(this IServiceCollection services, IEnumerable<Type> types)
    {
        types = types.Where(t =>
            t.IsAssignableTo(typeof(ICleanBaseScoped)) ||
            (!t.IsAssignableTo(typeof(ICleanBaseLifeTime)) &&
            !t.IsAssignableTo(typeof(ICleanBaseIgnore))));

        foreach (Type type in types)
        {
            IEnumerable<Type> interfaces = GetImplementedInterfaces(type);
            foreach (Type interfaceType in interfaces)
            {
                services.AddScoped(interfaceType, type);
            }
        }

        services.AddCleanUnitOfWork(types);

        return services;
    }

    #region Helpers
    private static IEnumerable<Type> GetWithInterfaceClasses(this IEnumerable<Type> enumerable)
    {
        return enumerable
            .Where(t =>
            t is { IsInterface: false, IsAbstract: false, IsClass: true } &&
            t.GetInterfaces().Length > 0 &&
            !t.Name.Contains('<') &&
            !t.Name.Contains(">d__") &&
            !t.Name.Contains(">b__"));
    }
    private static IEnumerable<Type> GetImplementedInterfaces(Type type)
    {
        var implementedInterfaces = type
            .GetInterfaces()
            .Where(i =>
            !i.Equals(typeof(ICleanBaseLifeTime)) &&
            !i.Equals(typeof(ICleanBaseScoped)) &&
            !i.Equals(typeof(ICleanBaseSingleton)) &&
            !i.Equals(typeof(ICleanBaseTransient)) &&
            !i.Equals(typeof(ICleanBaseIgnore)) &&
            !i.Equals(typeof(ICleanBaseMiddleware)) &&
            !i.Equals(typeof(ICleanBaseException)) &&
            !i.Equals(typeof(IDisposable)) &&
            !i.Equals(typeof(IAsyncDisposable)) &&
            !i.Equals(typeof(IEnumerable)) &&
            !i.IsAssignableTo(typeof(IEnumerable)) &&
            (!i.IsGenericType || !i.GetGenericTypeDefinition().Equals(typeof(IAsyncEnumerable<>))) &&
            !i.Equals(typeof(IEnumerator)) &&
            (!i.IsGenericType || !i.GetGenericTypeDefinition().Equals(typeof(IEnumerator<>))) &&
            (!i.IsGenericType || !i.GetGenericTypeDefinition().Equals(typeof(IAsyncEnumerator<>))) &&
            (!i.IsGenericType || !i.GetGenericTypeDefinition().Equals(typeof(IEquatable<>))) &&
            !i.Equals(typeof(IComparable)) &&
            (!i.IsGenericType || !i.GetGenericTypeDefinition().Equals(typeof(IComparable<>))) &&
            !i.Equals(typeof(IFormattable)) &&
            !i.Equals(typeof(ISpanFormattable)) &&
            !i.Equals(typeof(IUtf8SpanFormattable)) &&
            !i.Equals(typeof(IComparer)) &&
            (!i.IsGenericType || !i.GetGenericTypeDefinition().Equals(typeof(IComparer<>))) &&
            !i.Equals(typeof(IConvertible)) &&
            (!i.IsGenericType || !i.GetGenericTypeDefinition().Equals(typeof(IObservable<>))) &&
            (!i.IsGenericType || !i.GetGenericTypeDefinition().Equals(typeof(IObserver<>))) &&
            (!i.IsGenericType || !i.GetGenericTypeDefinition().Equals(typeof(IParsable<>))) &&
            (!i.IsGenericType || !i.GetGenericTypeDefinition().Equals(typeof(ISpanParsable<>))) &&
            (!i.IsGenericType || !i.GetGenericTypeDefinition().Equals(typeof(IUtf8SpanParsable<>))) &&
            !i.Equals(typeof(IQueryProvider)) &&
            !i.Equals(typeof(IServiceProvider)) &&
            (!i.IsGenericType || !i.GetGenericTypeDefinition().Equals(typeof(IServiceProviderFactory<>))) &&
            !i.Equals(typeof(IServiceScope)) &&
            !i.Equals(typeof(IServiceScopeFactory)) &&
            !i.Equals(typeof(IStructuralComparable)) &&
            !i.Equals(typeof(IStructuralEquatable)) &&
            !i.Equals(typeof(ISupportRequiredService)) &&
            !i.Equals(typeof(IThreadPoolWorkItem)) &&
            !i.Equals(typeof(ITimer)) &&
            !i.Equals(typeof(IAsyncResult)) &&
            !i.Equals(typeof(ICloneable)) &&
            !i.Equals(typeof(IMiddleware)) &&
            (!i.IsGenericType || !i.GetGenericTypeDefinition().Equals(typeof(IPipelineBehavior<,>))) &&
            !i.Equals(typeof(IHostedService))
            );

        return implementedInterfaces.Except(implementedInterfaces.SelectMany(i => i.GetInterfaces()));
    }

    #endregion
}
