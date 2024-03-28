using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Core.Caching.IRepositories;

public interface ICleanBaseMemoryCache<TKey, TValue>
    where TKey : notnull
    where TValue : class
{
    TValue Set(TKey key, TValue value);
    TValue Set(TKey key, TValue value, DateTimeOffset absoluteExpiration);
    TValue Set(TKey key, TValue value, TimeSpan absoluteExpirationRelativeToNow);
    TValue Set(TKey key, TValue value, IChangeToken expirationToken);
    TValue Set(TKey key, TValue value, MemoryCacheEntryOptions? options);
    TValue? Get(TKey key);
    bool TryGetValue(TKey key, out TValue? value);
    TValue? GetOrCreate(TKey key, Func<ICacheEntry, TValue> factory);
    Task<TValue?> GetOrCreateAsync(TKey key, Func<ICacheEntry, Task<TValue>> factory);
    ICacheEntry CreateEntry(TKey key);
    void Remove(TKey key);
}
