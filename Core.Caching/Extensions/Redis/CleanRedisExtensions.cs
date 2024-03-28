using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace Core.Caching.Extensions.Redis;

internal static class CleanRedisExtensions
{
    internal static IServiceCollection AddCleanRedis(this IServiceCollection services, IConfigurationSection redisSection)
    {
        var redisConfigurations = new RedisConfiguration();
        redisSection.Bind(redisConfigurations);

        services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfigurations);
        return services;
    }
}
