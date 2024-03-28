using Core.Caching.Exceptions;
using Core.Caching.IRepositories;
using Core.Domain.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Core.Caching.Repositories;

public abstract class CleanBaseMemoryCache<TKey, TValue> : ICleanBaseMemoryCache<TKey, TValue>
    where TKey : notnull
    where TValue : class
{
    private readonly IMemoryCache _memoryCache;

    protected CleanBaseMemoryCache(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    protected abstract string CachePrefixKey { get; }

    protected virtual string GetCacheKey(TKey key)
    {
        return $"{CachePrefixKey}_{key}";
    }

    public ICacheEntry CreateEntry(TKey key)
    {
        string cacheKey = CheckKeyNullity(key);
        return _memoryCache.CreateEntry(cacheKey);
    }

    public TValue? Get(TKey key)
    {
        string cacheKey = CheckKeyNullity(key);
        return _memoryCache.Get<TValue?>(cacheKey);
    }

    public TValue? GetOrCreate(TKey key, Func<ICacheEntry, TValue> factory)
    {
        string cacheKey = CheckKeyNullity(key);
        return _memoryCache.GetOrCreate(cacheKey, factory);
    }

    public async Task<TValue?> GetOrCreateAsync(TKey key, Func<ICacheEntry, Task<TValue>> factory)
    {
        string cacheKey = CheckKeyNullity(key);
        return await _memoryCache.GetOrCreateAsync(cacheKey, factory);
    }

    public void Remove(TKey key)
    {
        string cacheKey = CheckKeyNullity(key);
        _memoryCache.Remove(cacheKey);
    }

    public TValue Set(TKey key, TValue value)
    {
        string cacheKey = CheckKeyNullity(key);
        CheckValueNullity(value);
        return _memoryCache.Set(cacheKey, value);
    }

    public TValue Set(TKey key, TValue value, DateTimeOffset absoluteExpiration)
    {
        string cacheKey = CheckKeyNullity(key);
        CheckValueNullity(value);
        return _memoryCache.Set(cacheKey, value, absoluteExpiration);
    }

    public TValue Set(TKey key, TValue value, TimeSpan absoluteExpirationRelativeToNow)
    {
        string cacheKey = CheckKeyNullity(key);
        CheckValueNullity(value);
        return _memoryCache.Set(cacheKey, value, absoluteExpirationRelativeToNow);
    }

    public TValue Set(TKey key, TValue value, IChangeToken expirationToken)
    {
        string cacheKey = CheckKeyNullity(key);
        CheckValueNullity(value);
        return _memoryCache.Set(cacheKey, value, expirationToken);
    }

    public TValue Set(TKey key, TValue value, MemoryCacheEntryOptions? options)
    {
        string cacheKey = CheckKeyNullity(key);
        CheckValueNullity(value);
        return _memoryCache.Set(cacheKey, value, options);
    }

    public bool TryGetValue(TKey key, out TValue? value)
    {
        string cacheKey = CheckKeyNullity(key);
        return _memoryCache.TryGetValue(cacheKey, out value);
    }

    #region Private

    private string CheckKeyNullity(TKey key)
    {
        string cacheKey = GetCacheKey(key);

        if (string.IsNullOrWhiteSpace(cacheKey))
        {
            throw new CleanTemplateCachingInternalException(
                "An error occured in CleanBaseMemoryCache. Memory Cache: Key could not be null, empty or white space!",
                CleanBaseExceptionCode.CleanBaseMemoryCachingException,
                new CleanTemplateCachingInternalException(
                    "Memory Cache: Key could not be null, empty or white space!",
                    CleanBaseExceptionCode.ArgumentNullException));
        }

        return cacheKey;
    }

    private static void CheckValueNullity(TValue value)
    {
        if (value is null)
        {
            throw new CleanTemplateCachingInternalException(
                "An error occured in CleanBaseMemoryCache. Memory Cache: Value could not be null!",
                CleanBaseExceptionCode.CleanBaseMemoryCachingException,
                new CleanTemplateCachingInternalException(
                    "Memory Cache: Value could not be null!",
                    CleanBaseExceptionCode.ArgumentNullException));
        }
    }

    #endregion
}
