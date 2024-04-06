using Core.Messaging.Subscribers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Messaging.Extensions.Subscriber;

internal static class CleanSubscriberExtensions
{
    internal static IServiceCollection AddCleanSubscriber(this IServiceCollection services, IEnumerable<Type> types)
    {
        var subscribers = types
            .Where(t =>
            t.IsAssignableTo(typeof(CleanBaseRabbitMqSubscriber)) &&
            t is { IsInterface: false, IsAbstract: false, IsClass: true } &&
            (!t.IsGenericType || !t.GetGenericTypeDefinition().Equals(typeof(CleanBaseRabbitMqSubscriber<,>))));

        MethodInfo[] addHostedServiceMethods = typeof(ServiceCollectionHostedServiceExtensions)
            .GetMethods()
            .Where(m => m.Name == "AddHostedService" && m.IsGenericMethod)
            .ToArray();

        MethodInfo addHostedServiceMethod = addHostedServiceMethods
            .FirstOrDefault(m => m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(IServiceCollection))!;

        foreach (var subscriber in subscribers)
        {
            MethodInfo genericMethod = addHostedServiceMethod.MakeGenericMethod(subscriber);

            genericMethod.Invoke(null, new object[] { services });
        }

        return services;
    }
}
