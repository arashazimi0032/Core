# Clean Architecture Base Template for Caching <img src="../icon.png" height="40" width="40"/>

[![NuGet Version](https://img.shields.io/nuget/v/CleanTemplate.Caching)](https://www.nuget.org/packages/CleanTemplate.Caching)  [![NuGet Downloads](https://img.shields.io/nuget/dt/CleanTemplate.Caching)](https://www.nuget.org/packages/CleanTemplate.Caching)  [![GitHub Release](https://img.shields.io/github/v/release/arashazimi0032/Core)](https://github.com/arashazimi0032/Core/releases)  [![GitHub Tag](https://img.shields.io/github/v/tag/arashazimi0032/Core)](https://github.com/arashazimi0032/Core/tags)  [![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/arashazimi0032/Core/dotnet-desktop.yml)](https://github.com/arashazimi0032/Core/actions/workflows/dotnet-desktop.yml)  [![GitHub last commit](https://img.shields.io/github/last-commit/arashazimi0032/Core)](https://github.com/arashazimi0032/Core/commits/master/)    [![GitHub commit activity](https://img.shields.io/github/commit-activity/m/arashazimi0032/Core)](https://github.com/arashazimi0032/Core/commits/master/)   [![GitHub Issues or Pull Requests](https://img.shields.io/github/issues/arashazimi0032/Core)](https://github.com/arashazimi0032/Core/issues) [![GitHub Issues or Pull Requests](https://img.shields.io/github/issues-pr/arashazimi0032/Core)](https://github.com/arashazimi0032/Core/pulls)  [![GitHub top language](https://img.shields.io/github/languages/top/arashazimi0032/Core)](https://github.com/arashazimi0032/Core)
---

This package is a Clean Architecture Base Template comprising all Baseic and Abstract and Contract types for Redis and Memory Caching.

This package is one of the *[Cleantemplate](https://www.nuget.org/packages/CleanTemplate)* side packages which provides the basic requirements for creating ``Redis`` or ``Memory`` Caches.

The .Net version used in this project is net8.0

<p align="center" width="100%">
<img src="../icon.png" height="128" width="128"/>
</p>

# Contents

- [Dependencies](https://github.com/arashazimi0032/Core/tree/master/Core.Caching#dependencies)
- [Installation](https://github.com/arashazimi0032/Core/tree/master/Core.Caching#installation)
- [Usage](https://github.com/arashazimi0032/Core/tree/master/Core.Caching#usage)
    * [Dependency Injection and Add Caching to the project](https://github.com/arashazimi0032/Core/tree/master/Core.Caching#dependency-injection-and-add-caching-to-the-project)
    * [Redis Cache](https://github.com/arashazimi0032/Core/tree/master/Core.Caching#redis-cache)
    * [Memory Cache](https://github.com/arashazimi0032/Core/tree/master/Core.Caching#memory-cache)

   
# Dependencies

## net8.0
- CleanTemplate (>= 7.0.0)
- StackExchange.Redis (>= 2.7.33)
- StackExchange.Redis.Extensions.AspNetCore (>= 10.2.0)
- StackExchange.Redis.Extensions.Core (>= 10.2.0)
- StackExchange.Redis.Extensions.Newtonsoft (>= 10.2.0)

# Installation

.Net CLI

```
dotnet add package CleanTemplate.Caching --version x.x.x
```

Package Manager

```
NuGet\Install-Package CleanTemplate.Caching -Version x.x.x
```

Package Reference

```
<PackageReference Include="CleanTemplate.Caching" Version="x.x.x" />
```

Paket CLI

```
paket add CleanTemplate.Caching --version x.x.x
```

Script & Interactive

```
#r "nuget: CleanTemplate.Caching, x.x.x"
```

Cake

```
// Install CleanTemplate.Caching as a Cake Addin
#addin nuget:?package=CleanTemplate.Caching&version=x.x.x

// Install CleanTemplate.Caching as a Cake Tool
#tool nuget:?package=CleanTemplate.Caching&version=x.x.x
```

# Usage

I tried to follow the basic format and structure of ``CleanTemplate`` here.

## Dependency Injection and Add Caching to the project
To use from ``CleanTemplate.Caching`` in your project you should first register it in ``Program.cs`` as below:

1- First in ``appsettings.json`` add a section for redis configuration which is matched with ``StackExchange.Redis.Extensions.Core.Configuration.RedisConfiguration``:

```
"Redis": {
    "Hosts": [
      {
        "Host": "localhost",
        "Port": 6379
      }
    ]
  }
```

2- Add below Extension Method (``AddCleanCaching(IConfigurationSection redisSection)``) to ``Program.cs``:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCleanTemplate();

builder.Services.AddCleanCaching(builder.Configuration.GetSection("Redis"))

var app = builder.Build();

app.UseCleanTemplate();

app.Run();
```

***Notable Point:*** Note that to use the ``CleanTemplate.Caching`` package, you must have registered the main ``CleanTemplate`` package beforehand.

## Redis Cache

To use **Base Redis Cache**, you have to do exactly the same thing as using **Base Repository** with the difference that there is no **UnitOfWork** here.
You should create an interface that inherit from ``ICleanBaseRedisCache<TKey, TValue>`` and create a Class that inherit from ``CleanBaseRedisCache<TKey, TValue>`` and also implements its interface:

```csharp
public interface IProductRedisCache : ICleanBaseRedisCache<string, Product>
{
}

public class ProductRedisCache : CleanBaseRedisCache<string, Product>, IProductRedisCache
{
    public ProductRedisCache(IRedisDatabase database) : base(database)
    {
    }

    protected override string CachePrefixKey => "Product";
}
```

Now you can directly inject this service inside of your services and use it.

```csharp
public class ProductController : ControllerBase
{
    private readonly IProductRedisCache _redisCache;
    public ProductController(IProductRedisCache redisCache)
    {
        _redisCache = redisCache;
    }
}
```

All basic Methods implemented in ``CleanBaseRedisCache`` are as below:

```csharp
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
```

## Memory Cache

To use **Memory Cache**, you should operate exactly the same as **Redis Cache**.
You should create an interface that inherit from ``ICleanBaseMemoryCache<TKey, TValue>`` and create a Class that inherit from ``CleanBaseMemoryCache<TKey, TValue>`` and also implements its interface:

```csharp
public interface IProductMemoryCache : ICleanBaseMemoryCache<string, Product>
{
}

public class ProductMemoryCache : CleanBaseMemoryCache<string, Product>, IProductMemoryCache
{
    public ProductMemoryCache(IMemoryCache memoryCache) : base(memoryCache)
    {
    }

    protected override string CachePrefixKey => "Product";
}
```

Now you can directly inject this service inside of your services and use it.

```csharp
public class ProductController : ControllerBase
{
    private readonly IProductMemoryCache _memoryCache;
    public ProductController(IProductMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
}
```

All basic Methods implemented in ``CleanBaseMemoryCache`` are as below:

```csharp
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
```
