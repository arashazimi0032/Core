using Core.Messaging.Publishers;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Messaging.Extensions.Publisher;

internal static class CleanPublisherExtensions
{
    internal static IServiceCollection AddCleanPublisher(this IServiceCollection services, IEnumerable<Type> types)
    {
        var publishers = types
            .Where(t =>
            t.IsAssignableTo(typeof(CleanBaseRabbitMqPublisher)) &&
            t is { IsInterface: false, IsAbstract: false, IsClass: true } &&
            (!t.IsGenericType || !t.GetGenericTypeDefinition().Equals(typeof(CleanBaseRabbitMqPublisher<,>))));

        foreach (var publisher in publishers)
        {
            services.AddScoped(publisher);
        }

        return services;
    }
}
