# Clean Architecture Base Template 
![NuGet Downloads](https://img.shields.io/nuget/dt/CleanTemplate)   ![NuGet Version](https://img.shields.io/nuget/v/CleanTemplate)

This package is a Clean Architecture Base Template comprising all Baseic and Abstract and Contract types for initiating and structuring a .Net project.

This package provides you with all the necessary prerequisites to start a .Net project so that you don't have to be involved in building the infrastructure and foundation for the project.

The .Net version used in this project is net8.0

[![CleanTemplate](https://github.com/arashazimi0032/Core/blob/master/icon.png)](https://www.nuget.org/packages/CleanTemplate#readme-body-tab)

## Dependencies

### net8.0
- Carter (>= 8.0.0)
- MediatR (>= 12.2.0)
- Microsoft.AspNetCore.Http.Abstractions (>= 2.2.0)
- Microsoft.AspNetCore.Identity.EntityFrameworkCore (>= 8.0.2)
- Microsoft.EntityFrameworkCore (>= 8.0.2)
- Microsoft.EntityFrameworkCore.Relational (>= 8.0.2)
- Microsoft.EntityFrameworkCore.SqlServer (>= 8.0.2)

## Installation 

.Net CLI
```
dotnet add package CleanTemplate --version x.x.x
```
Package Manager
```
NuGet\Install-Package CleanTemplate -Version x.x.x
```
Package Reference
```
<PackageReference Include="CleanTemplate" Version="x.x.x" />
```
Paket CLI
```
paket add CleanTemplate --version x.x.x
```
Script & Interactive
```
#r "nuget: CleanTemplate, x.x.x"
```
Cake
```
// Install CleanTemplate as a Cake Addin
#addin nuget:?package=CleanTemplate&version=x.x.x

// Install CleanTemplate as a Cake Tool
#tool nuget:?package=CleanTemplate&version=x.x.x
```

## Usage

I tryed to use CleanBase Prefix for all Base types in CleanTemplate Package so you can easily find them.

### Dependency Injection And Service Configurations / Registerations

CleanTemplate Package has two Extension Method for configuring and registering all the types that are introduced in the rest of this tutorial.


To use all the capabilities provided by CleanTemplate, just do the following configurations so that all services, middleware, repositories, MediatR requests, etc. are automatically registered.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCleanTemplate();

var app = builder.Build();

app.UseCleanTemplate();

app.Run();
```
With adding ``builder.Services.AddCleanTemplate()`` and ``app.UseCleanTemplate();`` lines of code to your ``Program.cs``, all CleanTemplate capabilities will be registered except DbContext.
fir registering DbContext you should normally register it using ``builder.Services.AddDbContext`` and ``builder.Services.AddIdentity``.

The CleanTemplate Package introduces four empty interfaces (``ICleanBaseScoped``, ``ICleanBaseSingleton``, ``ICleanBaseTransient``, ``ICleanBaseIgnore``) for handling LifeTime Service Registration.

- if you want to register a service as *Singleton*, that service should inherit from ``ICleanBaseSingleton``.
- if you want to register a service as *Transient*, that service should inherit from ``ICleanBaseTransient``.
- if you want to register a service as *scoped*, that service must either inherit from ``ICleanBaseScoped`` or not inherit from any of the interfaces mentioned above. Classes that have an interface and neither themselves nor their interface inherit from any of the four above interfaces are injected as *scoped*.
- if you don't want to register a service as any of the above LifeTimes, that service **must** inherit from ``ICleanBaseIgnore``; otherwise, it will be automatically registered as *scoped*.


***Notable point*** : In order to automatically register services, it is necessary that your services have an interface, because the only class is never registered, except for the UnitOfWork class, which should not have an interface and is automatically registered.

***suggestion*** : To register services as any of the above types, it is better if your service interface inherits from one of the above interfaces, not your class (Implementation).

```csharp
public interface IService : ICleanBaseTransient
{
    void SayHello();
}

public class Service : IService
{
    public void SayHello()
    {
        Console.WriteLine("Hello World");
    }
}
```
The above service automatically registerd as below (you don't need to write the line of code below):
```csharp
builder.Services.AddTransient<IService, Service>();
```

### Dependency Injection And Service Configurations / Registerations From another Assembly

If you have **Assemblies** other than the Web API Assembly in your solution (for example, several *ClassLibraries* to which the Web API has reference), and you have services in these Assemblies that you want to be Registered as a LifeTime service, ``AddCleanTemplate`` cannot Register them from Assemblies other than Web API. Therefore, you must Register those Assemblies in the following way:

1- Install CleanTemplate Package inside that Assembly.
2- Create a DIModule inside that Assembly.

```csharp
public class MyAssemblyNameDIModule : CleanBaseDIModule
{
}
```
3- Add this DIModule to Registration Flow inside Web API Program.cs After AddCleanTemplate().
```csharp
builder.Services.AddCleanTemplateDIModule<MyAssemblyNameDIModule>();
```
Now all the services inside this assembly are injected as a LifetTme Service. (Any type of LifeTime service that you set according to the rules of the LifeTime Service Registeration mentioned above).

### Create Model

The Generic ``CleanBaseEntity<T>`` used for creating an entity with Id's type as generic parameter. All structural equality constraints implemented for this type.
The CleanBaseEntity class also has predefined `CreatedAt` and `ModifiedAt` properties that automatically filled in SaveChangesAsync.
```csharp
public class Product : CleanBaseEntity<Guid>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```
If you want to use strongly typeed Id you can use ``CleanBaseValueObject`` record.
```csharp
public record ProductId(string Name, decimal Price) : CleanBaseValueObject;

public class Product : CleanBaseEntity<ProductId>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```
Also you can use ``CleanBaseAggregateRoot<TId>`` to follow DDD concepts. The TId should be a ``CleanBaseValueObject``.

### Create DbContext
There are two Base DbContext implemented in CleanTemplate library; ``CleanBaseDbContext<TContext>`` and ``CleanBaseIdentityDbContext<TContext, TUser>``.
SaveChangesAsync overrided for them to Automatically Auditing and Publishing Domain Events of type ``ICleanBaseDomainEvent``.

```csharp
public class ApplicationDbContext : CleanBaseDbContext<ApplicationDbContext>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher)
        : base(options, publisher)
    {
    }

    public DbSet<Product> Products { get; set; }

}
```
``IPublisher`` automatically injected for publishing domain events.

for configuring DbContext you should add AddDbContext in your Program.cs. (this part doesn't handled automatically in CleanTemplate)
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
```
you can use ``CleanBaseIdentityDbContext<TContext, TUser>`` similar to ``CleanBaseDbContext<TContext>``.

### Entity Configuration
For Entity Configuration you can use ``ICleanBaseEntityConfiguration<TEntity>``. All configs automatically handled.
```csharp
public class ProductConfiguration : ICleanBaseEntityConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name).IsRequired().HasMaxLength(20);
    }
}
```

### Repositories
Repositories splitted to Query and Command Repositories based on CQRS Concepts.
For use of Base Repositories do as follows.
```csharp
public interface IProductCommandRepository : ICleanBaseCommandRepository<Product>
{
}

public interface IProductQueryRepository : ICleanBaseQueryRepository<Product, Guid>
{
}

public class ProductCommandRepository : CleanBaseCommandRepository<ApplicationDbContext, Product>, IProductCommandRepository
{
    public ProductCommandRepository(ApplicationDbContext context) : base(context)
    {
    }
}

public class ProductQueryRepository : CleanBaseQueryRepositry<ApplicationDbContext, Product, Guid>, IProductQueryRepository
{
    public ProductQueryRepository(ApplicationDbContext context) : base(context)
    {
    }
}
```

Then you should write command and query UnitOfWorks as below.
```csharp
public interface ICommandUnitOfWork : ICleanBaseCommandUnitOfWork
{
    IProductCommandRepository Products { get; }
}

public interface IQueryUnitOfWork : ICleanBaseQueryUnitOfWork
{
    IProductQueryRepository Products { get; }
}

public class CommandUnitOfWork : CleanBaseCommandUnitOfWork<ApplicationDbContext>, ICommandUnitOfWork
{
    public CommandUnitOfWork(ApplicationDbContext context) : base(context)
    {
    }

    public IProductCommandRepository Products {  get; private set; }
}

public class QueryUnitOfWork : CleanBaseQueryUnitOfWork<ApplicationDbContext>, IQueryUnitOfWork
{
    public QueryUnitOfWork(ApplicationDbContext context) : base(context)
    {
    }

    public IProductQueryRepository Products { get; private set; }
}
```
Finally for use of all of them you should write UnitOfWork.
```csharp
public class UnitOfWork : CleanBaseUnitOfWork<ApplicationDbContext, CommandUnitOfWork, QueryUnitOfWork>
{
    public UnitOfWork(ApplicationDbContext context) : base(context)
    {
    }
}
```

Now for use of UnitOfWork in a TestService you can inject Just UnitOfWork and use it.
```csharp
public class TestService
{
    private readonly UnitOfWork _unitOfWork;

    public TestService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task TestMethod(Product product)
    {
        Product result = await _unitOfWork.Commands.Products.AddAsync(product);
        bool isSuccess = await _unitOfWork.SaveChangesAsync();
    }
}
```

All basic Transactions implemented in Base Repositories and you can use them.

**Commands**
```csharp
Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
Task AddRangeAsync(params TEntity[] entities);
Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
TEntity Add(TEntity entity);
void AddRange(params TEntity[] entities);
void AddRange(IEnumerable<TEntity> entities);
TEntity Update(TEntity entity);
void UpdateRange(params TEntity[] entity);
void UpdateRange(IEnumerable<TEntity> entity);
TEntity Remove(TEntity entity);
void RemoveRange(params TEntity[] entity);
void RemoveRange(IEnumerable<TEntity> entity);
```

**Queries**
```csharp
Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
TEntity? GetById(TId id);
Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
TEntity? Get(Expression<Func<TEntity, bool>> predicate);
Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
IEnumerable<TEntity> GetAll();
Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
Task<CleanPaginatedList<TEntity>> GetPaginatedAsync(CleanPage page, CancellationToken cancellationToken = default);
CleanPaginatedList<TEntity> GetPaginated(CleanPage page);
Task<CleanPaginatedList<TEntity>> GetPaginatedAsync(Expression<Func<TEntity, bool>> predicate, CleanPage page, CancellationToken cancellationToken = default);
CleanPaginatedList<TEntity> GetPaginated(Expression<Func<TEntity, bool>> predicate, CleanPage page);
IQueryable<TEntity> GetQueryable();
IQueryable<TEntity> GetQueryableAsNoTrack();
```

### MediatR Request and RequestHandlers
We use of MediatR to implement CRUD operations instead of services.
```csharp
public record CreateProductCommand(string Name, decimal Price) : ICleanBaseCommand<Product>;

internal sealed class CreateProductCommandHandler : ICleanBaseCommandHandler<CreateProductCommand, Product>
{
    private readonly UnitOfWork _unitOfWork;

    public CreateProductCommandHandler(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(request.Name, request.Price);

        var result = await _unitOfWork.Commands.Products.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return result;
    }
}
```
Then in controller: 
```csharp
public class ProductController : ControllerBase
{
    private readonly ISender _sender;
    public ProductController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("CreateProduct")]
    public async Task<IActionResult> CreateProduct(CreateProductRequest request)
    {
        var command = new CreateProductCommand(request.Name, request.Price);
        var product = await _sender.Send(command);
        return Ok(product);
    }
}
```
Also for Queries you can use of ``ICleanBaseQuery`` and ``ICleanBaseQueryHandler``.

### Domain Events

You can create a Domain Event and it's handler so CleanTemplate automatically config and publishing them in SaveChangesAsync.
```csharp
public record ProductCreatedDomainEvent(string Name) : ICleanBaseDomainEvent;

public class ProductCreatedDomainEventHandler : ICleanBaseDomainEventHandler<ProductCreatedDomainEvent>
{
    public async Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync($"Product {notification.Name} Created Successfully. I Am a Domain Event!");
    }
}
```
Then you should Raise it in Product Creation as below and then CleanTemplate automatically publishing it:
```csharp
public class Product : CleanBaseEntity<Guid>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public static Product Create(string name, decimal price)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Price = price
        };

        product.Raise(new ProductCreatedDomainEvent(product.Name));

        return product;
    }
}
```

### Pagination

The CleanTemplate introduces a ``CleanPaginatedList<T>`` for get data in pagination format with below properties:
```csharp
public List<T> Items { get; set; }
public CleanPage Page { get; set; } = new CleanPage();
public long TotalCount { get; set; }
public int TotalPages { get; set; }
public bool HasNextPage => Page.Number * Page.Size < TotalCount;
public bool HasPreviousPage => Page.Number > 1;
```

``CleanPage`` is a format for give *PageNumber* and *PageSize* to PaginatedList.
```csharp
public class CleanPage
{
    public int Number { get; set; } = 1;
    public int Size { get; set; } = 10;
}
```

We also implemented ``ToPaginatedListAsync(CleanPage Page)`` and ``ToPaginatedList(CleanPage Page)`` extension methods for IQueryable and IEnumerable.

### Exceptions

``CleanBaseException`` & ``CleanBaseException<TCode>`` implemented as a base exception class to Handling Exceptions. where the TCode is an enum to set an specific in-App ExceptionCode for every Exception and then you can handle Exception Messages in other languages using *.resx* files in C#.
When you use this type of exception, GlobalExceptionHandling of CleanTemplate Package will recognize it and give you more complete details of the error that occurred.

```csharp
public class TestException : CleanBaseException<ExceptionStatusCode>
{
    public override ExceptionStatusCode ExceptionType => ExceptionStatusCode.CoreResource;
    
    public override string DefaultMessage => "This Is Test Exception";
}
public enum ExceptionStatusCode
{
    CoreResource
}
```

### Logging to Elasticsearch

We use **Serilog** and **ElasticSearch** for logging all Exceptions. By default, the CleanTemplate GlobalExceptionHandling middleware records all logs only in Elasticsearch using serilog.
To take advantage of the ``Logging to Elasticsearch`` feature in the CleanTemplate package, you must make the following settings. These settings are the minimum possible settings to benefit from the logging capability in the **CleanTemplate**. If you need more settings for **Srilog**, you can add them in the *appsettings.json*

1- In the ``appsettings.json`` add below settings:
```
"Serilog": {
    "MinimumLevel": {
      "Default": "Error",
      "Override": {
        "Microsoft": "Error",
        "System": "Error"
      }
    }
  },
  "ElasticConfiguration": {
    "Uri": "http://localhost:9200"
  }
```
2- in the ``program.cs`` add below configuration:
```csharp
builder.AddCleanLogger(builder.Configuration["ElasticConfiguration:Uri"]);
```

With above configurations, all error logs automatically records in Elasticsearch and you should see them in **Kibana** Dashboard.

### Settings

``CleanBaseSetting`` class added for reading and configuring settings from appsettings.json.
Make sure that the name of the settings section in appsettings.json must be equal to the name of your setting class.

1- Add a setting in *appsettings.json*:
```csharp
{
  "TestSetting": {
    "Name": "Arash",
    "Email": "arashazimi0032@gmail.com",
    "Age": 26
  }
}
```

2- Add A class with the same name as your setting class.
```csharp
public class TestSetting : CleanBaseSetting
{
    public TestSetting1(IConfiguration configuration) : base(configuration)
    {
    }

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
}
```

Then you just need to inject this TestSetting Class inside your services to access to its Properties.

### Middlewares

``ICleanBaseMiddleware`` implemented for automatically handling Middlewares in CleanTemplate.
```csharp
public class TestMiddleware : ICleanBaseMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await Console.Out.WriteLineAsync("I Am Middleware");
        await next(context);
    }
}
```

### Endpoints (Minimal APIs)

``ICleanBaseEndpoint`` interface is a base type for implementing Endpoints as below:
```csharp
public class ProductEndPoint : ICleanBaseEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/Endpoints/CreateProduct", async (CleanPage page, ISender sender) =>
        {
            var command = new CreateProductCommand(page);
            var product = await sender.Send(command);
            return Results.Ok(product);
        });
    }
}
```
