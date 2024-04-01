# Clean Architecture Base Template for GRPC Communication <img src="../icon.png" height="40" width="40"/>

[![NuGet Version](https://img.shields.io/nuget/v/CleanTemplate.Grpc)](https://www.nuget.org/packages/CleanTemplate.Grpc)  [![NuGet Downloads](https://img.shields.io/nuget/dt/CleanTemplate.Grpc)](https://www.nuget.org/packages/CleanTemplate.Grpc)  [![GitHub Release](https://img.shields.io/github/v/release/arashazimi0032/Core)](https://github.com/arashazimi0032/Core/releases)  [![GitHub Tag](https://img.shields.io/github/v/tag/arashazimi0032/Core)](https://github.com/arashazimi0032/Core/tags)  [![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/arashazimi0032/Core/dotnet-desktop.yml)](https://github.com/arashazimi0032/Core/actions/workflows/dotnet-desktop.yml)  [![GitHub last commit](https://img.shields.io/github/last-commit/arashazimi0032/Core)](https://github.com/arashazimi0032/Core/commits/master/)    [![GitHub commit activity](https://img.shields.io/github/commit-activity/m/arashazimi0032/Core)](https://github.com/arashazimi0032/Core/commits/master/)   [![GitHub Issues or Pull Requests](https://img.shields.io/github/issues/arashazimi0032/Core)](https://github.com/arashazimi0032/Core/issues) [![GitHub Issues or Pull Requests](https://img.shields.io/github/issues-pr/arashazimi0032/Core)](https://github.com/arashazimi0032/Core/pulls)  [![GitHub top language](https://img.shields.io/github/languages/top/arashazimi0032/Core)](https://github.com/arashazimi0032/Core)
---

This package is a Clean Architecture Base Template comprising all Baseic and Abstract and Contract types for **GRPC** Call between Microservices.

This package is one of the *[Cleantemplate](https://www.nuget.org/packages/CleanTemplate)* side packages which provides the basic requirements for Calling Microservices using ``GRPC`` and HTTP/2 Protocol.

The .Net version used in this project is net8.0

<p align="center" width="100%">
<img src="../icon.png" height="128" width="128"/>
</p>

# Contents

- [Dependencies](https://github.com/arashazimi0032/Core/tree/master/Core.Grpc#dependencies)
- [Installation](https://github.com/arashazimi0032/Core/tree/master/Core.Grpc#installation)
- [Usage](https://github.com/arashazimi0032/Core/tree/master/Core.Grpc#usage)
    * [Dependency Injection and Add Grpc to the projects](https://github.com/arashazimi0032/Core/tree/master/Core.Grpc#dependency-injection-and-add-grpc-to-the-projects)
    * [Create Contract](https://github.com/arashazimi0032/Core/tree/master/Core.Grpc#create-contract)
    * [Implement the Contract](https://github.com/arashazimi0032/Core/tree/master/Core.Grpc#implement-the-contract)
    * [Register Contract Implementation Inside of ServerWebAPI](https://github.com/arashazimi0032/Core/tree/master/Core.Grpc#register-contract-implementation-inside-of-serverwebapi)
    * [Create Client](https://github.com/arashazimi0032/Core/tree/master/Core.Grpc#create-client)
    * [Using the Client for GRPC Call](https://github.com/arashazimi0032/Core/tree/master/Core.Grpc#using-the-client-for-grpc-call)

   
# Dependencies

## net8.0
- CleanTemplate (>= 7.5.0)
- Grpc.Net.Client (>= 2.62.0)
- MessagePack (>= 2.5.140)
- ServiceModel.Grpc.AspNetCore (>= 1.8.2)
- System.ServiceModel.Primitives (>= 8.8.0)

# Installation

.Net CLI

```
dotnet add package CleanTemplate.Grpc --version x.x.x
```

Package Manager

```
NuGet\Install-Package CleanTemplate.Grpc -Version x.x.x
```

Package Reference

```
<PackageReference Include="CleanTemplate.Grpc" Version="x.x.x" />
```

Paket CLI

```
paket add CleanTemplate.Grpc --version x.x.x
```

Script & Interactive

```
#r "nuget: CleanTemplate.Grpc, x.x.x"
```

Cake

```
// Install CleanTemplate.Grpc as a Cake Addin
#addin nuget:?package=CleanTemplate.Grpc&version=x.x.x

// Install CleanTemplate.Grpc as a Cake Tool
#tool nuget:?package=CleanTemplate.Grpc&version=x.x.x
```

# Usage

I tried to follow the basic format and structure of ``CleanTemplate`` here.

For use of this package we need three project:
1- A **Client** Web API project as First Microservice: ClientWebAPI
2- A **Server** Web API project as Second Microservice: ServerWebAPI
3- A **Contract** ClassLibrary Project: Contracts

## Dependency Injection and Add Grpc to the projects
To use from ``CleanTemplate.Grpc`` in your project you should first register it in ``Program.cs`` of Client and Server projects as below:

1- First in ``appsettings.json`` file of **Server** project add a section for kestrel configuration:

```
  "Kestrel": {
    "EndpointDefaults": {
      "Protocols": "Http2"
    },
    "EndPoints": {
      "Https": {
        "Url": "https://*:<your project Https Port>"
      },
      "Http": {
        "Url": "http://*:<your project Http Port>"
      }
    }
  }
```

2- Add below Extension Method (``AddCleanGrpc()``) to ``Program.cs`` of both **Client** and **Server** projects:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCleanTemplate();

builder.Services.AddCleanGrpc()

var app = builder.Build();

app.Run();
```

***Notable Point:*** Note that to use the ``CleanTemplate.Grpc`` package, you must have registered the main ``CleanTemplate`` package beforehand.

3- Also you should Add a ``DIModule`` To your **Contracts** Project and Register it inside of just **ClientWebAPI** Project.

```csharp
// DIModule inside of your Contracts Project.

public class ContractDIModule : CleanBaseDIModule
{
}
```

```csharp
// Register ContractDIModule inside of ClientWebAPI Program.cs

builder.Services.AddCleanTemplate();

builder.Services.AddCleanTemplateDIModule<ContractDIModule>();
```

## Create Contract

You should create an interface inside of your **Contracts** project that inherit from ``ICleanBaseGrpcContract`` and set ``[ServiceContract]`` attribute for this interface and also set ``[OperationContract]`` attribute for its methods.
Also you should create Request and Response types inside of your **Contracts** project for your methods in which inheriting from ``CleanBaseGrpcMessage``.

```csharp
public class Request1 : CleanBaseGrpcMessage
{
    // Some Properties
}

public class Request2 : CleanBaseGrpcMessage
{
    // Some Properties
}

public class Response1 : CleanBaseGrpcMessage
{
    // Some Properties
}

public class Response2 : CleanBaseGrpcMessage
{
    // Some Properties
}
```

```csharp
[ServiceContract]
public interface IMyContract : ICleanBaseGrpcContract
{
    [OperationContract]
    Task<Response1> Method1(Request1 request1)

    [OperationContract]
    Task<Response2> Method2(Request2 request2)
}
```

## Implement the Contract

You have already created a contract in the **Contracts** project. Now you need to implement it in the **Server** project **(ServerWebAPI)**.

```csharp
public class MyContract : IMyContract
{
    public async Task<Response1> Method1(Request1 request1)
    {
        // some implementation
    }

    public async Task<Response2> Method2(Request2 request2)
    {
        // some implementation
    }
}
```

## Register Contract Implementation Inside of ServerWebAPI:

For automatically register all contract implementations you should use of ``MapCleanGrpc(Assembly ContractImplementationAssembly)`` extension method of ``IEndpointRouteBuilder`` and give it the assembly where the implementation of your contracts is located.

```csharp
assemblyOfContractImplementations = typeof(MyContract).Assembly;
app.MapCleanGrpc(assemblyOfContractImplementations);
```

## Create Client

Next step is create a client and its implementation for Grpc Call inside of your **Contract** project.
Your Client interface should inherit from ``ICleanBaseGrpcClient`` and its implementation should inherit from ``CleanBaseGrpcClient`` and its interface.
All of your Contracts should define inside of this client as properties and their implementations automatically created by Reflection..

```csharp
public interface IMyClient : ICleanBaseGrpcClient
{
    IMyContract MyContract { get; }
}
```

```csharp
public class MyClient : CleanBaseGrpcClient, IMyClient
{
    public MyClient(ICleanGrpcClientFactory cleanGrpcClientFactory, IConfiguration configuration)
        : base(cleanGrpcClientFactory, configuration)
    {
    }

    public IMyContract MyContract { get; private set; }
}
```

Also you should create a Section inside of ``appsettings.json`` file of your **ClientWebAPI** project with the section name of your client that has a ``ServerAddress`` field.

```
  "Kestrel": {
    "EndpointDefaults": {
      "Protocols": "Http2"
    },
    "EndPoints": {
      "Https": {
        "Url": "https://*:<your project Https Port>"
      },
      "Http": {
        "Url": "http://*:<your project Http Port>"
      }
    }
  },
  "MyClient": {
    "ServerAddress": "https://localhost:7228" // this is the Address of your destination Server.
  }
```

***Notable Point***: Note that your Client Class Name and its section Name inside of ``appsettings.json`` should be exactly equal.

***Important Advice***: you sholud create just one Client per Microservice and if you have more than one Contract in this Microservice, you should add them to the same Client.

```csharp
public interface IMyClient : ICleanBaseGrpcClient
{
    IMyContract1 MyContract1 { get; }

    IMyContract2 MyContract2 { get; }

    IMyContract3 MyContract3 { get; }

    IMyContract4 MyContract4 { get; }
}
```

## Using the Client for GRPC Call

Finally to Calling **ServerWebAPI** Microservice from **ClientWebAPI** Microservice with GRPC Protocol, you should inject the client you created, into the controllers or services of the **ClientWebAPI**.

```csharp
[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IMyClient _myClient;

    public ProductController(IMyClient myClient)
    {
        _myClient = myClient
    }

    [HttpPost("TestGrpc")]
    public async Task<IActionResult> TestGrpc(Request1 request1)
    {
        var response1 = await _myClient.MyContract.Method1(request1);
        return Ok(response1);
    }
}
```
