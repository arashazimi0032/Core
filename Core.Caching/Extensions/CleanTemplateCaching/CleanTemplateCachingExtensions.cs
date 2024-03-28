using Core.Caching.Extensions.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Caching.Extensions.CleanTemplateCaching;

public static class CleanTemplateCachingExtensions
{
    public static IServiceCollection AddCleanCaching(this IServiceCollection services, IConfigurationSection redisSection)
    {
        services
            .AddCleanRedis(redisSection)
            .AddMemoryCache();

        return services;
    }
}
