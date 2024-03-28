using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Core.Caching.IRepositories;

public interface ICleanBaseRedisCache<TKey, TValue>
    where TKey : notnull
    where TValue : class
{
    TValue? Get(TKey key);
    Task<TValue?> GetAsync(TKey key);
    Task<List<string>> GetAllKeysAsync();
    bool KeyExists(TKey key);
    Task<bool> KeyExistsAsync(TKey key);
    bool Remove(TKey key);
    Task<bool> RemoveAsync(TKey key);
    Task<long> RemoveAllKeysAsync();
    bool Set(TKey key, TValue value, TimeSpan? expiryTime = null);
    Task<bool> SetAsync(TKey key, TValue value, TimeSpan? expiryTime = null);
    bool Set(TKey key, TValue value, DateTimeOffset expiryAt);
    Task<bool> SetAsync(TKey key, TValue value, DateTimeOffset expiryAt);
    Task FlushDbAsync();
    IDatabase GetDatabase();
    IRedisDatabase GetRedisDatabase();
}
