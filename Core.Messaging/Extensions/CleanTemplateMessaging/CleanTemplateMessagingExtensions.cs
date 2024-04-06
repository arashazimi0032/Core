using Core.Messaging.ConnectionFactories;
using Core.Messaging.Extensions.Publisher;
using Core.Messaging.Extensions.Subscriber;
using Core.Messaging.Settings;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Messaging.Extensions.CleanTemplateMessaging;

public static class CleanTemplateMessagingExtensions
{
    public static IServiceCollection AddCleanMessaging(this IServiceCollection services, Assembly assembly)
    {
        IEnumerable<Type> types = assembly.GetTypes();

        services.AddSingleton<CleanRabbitMqHostSetting>();

        services.AddSingleton<ICleanRabbitMqConnectionFactory, CleanRabbitMqConnectionFactory>();

        services
            .AddCleanPublisher(types)
            .AddCleanSubscriber(types);

        return services;
    }
}
