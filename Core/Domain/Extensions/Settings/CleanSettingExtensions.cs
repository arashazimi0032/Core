using Core.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Domain.Extensions.Settings;

internal static class CleanSettingExtensions
{
    internal static IServiceCollection AddCleanSettings(this IServiceCollection services, IEnumerable<Type> types)
    {
        IEnumerable<Type> cleanSettings = types.Where(t => t.IsAssignableTo(typeof(CleanBaseSetting)));

        foreach (var setting in cleanSettings)
        {
            services.AddSingleton(setting);
        }

        return services;
    }
}
