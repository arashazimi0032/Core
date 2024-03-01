using Core.Application.ServiceLifeTimes;
using Core.Infrastructure.Repositories.UnitOfWork;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Reflection;

namespace Core.Domain.Extensions.LifeTime;

internal static class CleanLifeTimeExtensions
{
    #region Helper
    internal static IServiceCollection AddLifeTimeServices(this IServiceCollection services, Assembly assembly)
    {
        services
            .AddTransientServices(assembly)
            .AddSingletonServices(assembly)
            .AddScopedServices(assembly);

        return services;
    }
    private static IServiceCollection AddTransientServices(this IServiceCollection services, Assembly assembly)
    {
        IEnumerable<Type> types = assembly
            .GetTypes()
            .Where(t =>
            t.IsAssignableTo(typeof(ICleanBaseTransient)))
            .GetWithInterfaceClasses();

        foreach (Type type in types)
        {
            IEnumerable<Type> interfaces = GetImplementedInterfaces(type);
            foreach (Type interfaceType in interfaces)
            {
                services.AddTransient(interfaceType, type);
            }
        }

        return services;
    }
    private static IServiceCollection AddSingletonServices(this IServiceCollection services, Assembly assembly)
    {
        IEnumerable<Type> types = assembly
            .GetTypes()
            .Where(t =>
            t.IsAssignableTo(typeof(ICleanBaseSingleton)))
            .GetWithInterfaceClasses();

        foreach (Type type in types)
        {
            IEnumerable<Type> interfaces = GetImplementedInterfaces(type);
            foreach (Type interfaceType in interfaces)
            {
                services.AddSingleton(interfaceType, type);
            }
        }

        return services;
    }
    private static IServiceCollection AddScopedServices(this IServiceCollection services, Assembly assembly)
    {
        IEnumerable<Type> types = assembly
            .GetTypes()
            .Where(t => 
            t.IsAssignableTo(typeof(ICleanBaseScoped)) ||
            (!t.IsAssignableTo(typeof(ICleanBaseLifeTime)) &&
            !t.IsAssignableTo(typeof(ICleanBaseIgnore))))
            .GetWithInterfaceClasses();

        foreach (Type type in types)
        {
            IEnumerable<Type> interfaces = GetImplementedInterfaces(type);
            if (interfaces.Any())
            {
                foreach (Type interfaceType in interfaces)
                {
                    services.AddScoped(interfaceType, type);
                }
            }
            else
            {
                if (type.IsAssignableTo(typeof(CleanBaseUnitOfWork<,,>)))
                {
                    services.AddScoped(type);
                }
            }
            
        }

        return services;
    }
    private static IEnumerable<Type> GetWithInterfaceClasses(this IEnumerable<Type> enumerable)
    {
        return enumerable
            .Where(t =>
            t is { IsInterface: false, IsAbstract: false, IsClass: true }
            && t.GetInterfaces().Length > 0);
    }
    private static IEnumerable<Type> GetImplementedInterfaces(Type type)
    {
        return type
            .GetInterfaces()
            .Where(i =>
            !i.Equals(typeof(ICleanBaseLifeTime)) &&
            !i.Equals(typeof(ICleanBaseScoped)) &&
            !i.Equals(typeof(ICleanBaseSingleton)) &&
            !i.Equals(typeof(ICleanBaseTransient)) &&
            !i.Equals(typeof(ICleanBaseIgnore)) &&
            !i.Equals(typeof(IDisposable)) &&
            !i.Equals(typeof(IAsyncDisposable)) &&
            !i.Equals(typeof(IEnumerable)) &&
            !i.IsAssignableTo(typeof(IEnumerable)) &&
            !i.Equals(typeof(IAsyncEnumerable<>)) &&
            !i.Equals(typeof(IEnumerator)) &&
            !i.Equals(typeof(IEquatable<>)) &&
            !i.Equals(typeof(IComparable)) &&
            !i.Equals(typeof(IComparable<>)) &&
            !i.Equals(typeof(IFormattable)) &&
            !i.Equals(typeof(ICloneable)));
    }

    #endregion
}
