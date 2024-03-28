using Core.Caching.Exceptions;
using Core.Caching.IRepositories;
using Core.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Core.Caching.Repositories;

public abstract class CleanBaseRedisCache<TKey, TValue> : ICleanBaseRedisCache<TKey, TValue>
    where TKey : notnull
    where TValue : class
{
    private readonly IRedisDatabase _database;
    protected CleanBaseRedisCache(IRedisDatabase database)
    {
        _database = database;
    }

    protected abstract string CachePrefixKey { get; }

    protected virtual string GetCacheKey(TKey key)
    {
        return $"{CachePrefixKey}_{key}";
    }

    public TValue? Get(TKey key)
    {
        string cacheKey = CheckKeyNullity(key);

        RedisValue value = _database.Database.StringGet(cacheKey);
        if (!value.HasValue || value.IsNull)
        {
            return null;
        }
        return JsonConvert.DeserializeObject<TValue?>(value!);
    }

    public async Task<TValue?> GetAsync(TKey key)
    {
        string cacheKey = CheckKeyNullity(key);

        RedisValue value = await _database.Database.StringGetAsync(cacheKey);
        if (!value.HasValue || value.IsNull)
        {
            return null;
        }
        return JsonConvert.DeserializeObject<TValue?>(value!);
    }

    public virtual async Task<List<string>> GetAllKeysAsync()
    {
        string prefix = CachePrefixKey + "_*";
        return (await _database.SearchKeysAsync(prefix)).ToList();
    }

    public bool KeyExists(TKey key)
    {
        string cacheKey = CheckKeyNullity(key);

        return _database.Database.KeyExists(cacheKey);
    }

    public async Task<bool> KeyExistsAsync(TKey key)
    {
        string cacheKey = CheckKeyNullity(key);

        return await _database.Database.KeyExistsAsync(cacheKey);
    }

    public bool Remove(TKey key)
    {
        string cacheKey = CheckKeyNullity(key);

        return _database.Database.KeyDelete(cacheKey);
    }

    public async Task<bool> RemoveAsync(TKey key)
    {
        string cacheKey = CheckKeyNullity(key);

        return await _database.Database.KeyDeleteAsync(cacheKey);
    }

    public async Task<long> RemoveAllKeysAsync()
    {
        List<string> keys = await GetAllKeysAsync();
        if (keys is not null && keys.Count > 0)
        {
            return await _database.RemoveAllAsync([.. keys]);
        }
        return 0;
    }

    public bool Set(TKey key, TValue value, TimeSpan? expiryTime = null)
    {
        string cacheKey = CheckKeyNullity(key);

        CleanBaseRedisCache<TKey, TValue>.CheckValueNullity(value);

        string valueString = JsonConvert.SerializeObject(value);
        return _database.Database.StringSet(cacheKey, valueString, expiryTime);
    }

    public async Task<bool> SetAsync(TKey key, TValue value, TimeSpan? expiryTime = null)
    {
        string cacheKey = CheckKeyNullity(key);

        CleanBaseRedisCache<TKey, TValue>.CheckValueNullity(value);

        string valueString = JsonConvert.SerializeObject(value);
        return await _database.Database.StringSetAsync(cacheKey, valueString, expiryTime);
    }

    public bool Set(TKey key, TValue value, DateTimeOffset expiryAt)
    {
        var expiryTime = expiryAt.UtcDateTime.Subtract(DateTime.UtcNow);
        return Set(key, value, expiryTime);
    }

    public async Task<bool> SetAsync(TKey key, TValue value, DateTimeOffset expiryAt)
    {
        var expiryTime = expiryAt.UtcDateTime.Subtract(DateTime.UtcNow);
        return await SetAsync(key, value, expiryTime);
    }

    public async Task FlushDbAsync()
    {
        await _database.FlushDbAsync();
    }

    public IDatabase GetDatabase()
    {
        return _database.Database;
    }

    public IRedisDatabase GetRedisDatabase()
    {
        return _database;
    }

    #region Private
    private string CheckKeyNullity(TKey key)
    {
        string cacheKey = GetCacheKey(key);

        if (string.IsNullOrWhiteSpace(cacheKey))
        {
            throw new CleanTemplateCachingInternalException(
                "An error occured in CleanBaseRedisCache. Redis DataBase: Key could not be null, empty or white space!",
                CleanBaseExceptionCode.CleanBaseRedisCachingException,
                new CleanTemplateCachingInternalException(
                    "Redis DataBase: Key could not be null, empty or white space!",
                    CleanBaseExceptionCode.ArgumentNullException));
        }

        return cacheKey;
    }

    private static void CheckValueNullity(TValue value)
    {
        if (value is null)
        {
            throw new CleanTemplateCachingInternalException(
                "An error occured in CleanBaseRedisCache. Redis DataBase: Value could not be null!",
                CleanBaseExceptionCode.CleanBaseRedisCachingException,
                new CleanTemplateCachingInternalException(
                    "Redis DataBase: Value could not be null!",
                    CleanBaseExceptionCode.ArgumentNullException));
        }
    }
    #endregion
}
