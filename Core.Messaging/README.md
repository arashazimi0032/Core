# Clean Architecture Base Template for Messaging <img src="../icon.png" height="40" width="40"/>

[![NuGet Version](https://img.shields.io/nuget/v/CleanTemplate.Messaging)](https://www.nuget.org/packages/CleanTemplate.Messaging)  [![NuGet Downloads](https://img.shields.io/nuget/dt/CleanTemplate.Messaging)](https://www.nuget.org/packages/CleanTemplate.Messaging)  [![GitHub Release](https://img.shields.io/github/v/release/arashazimi0032/Core)](https://github.com/arashazimi0032/Core/releases)  [![GitHub Tag](https://img.shields.io/github/v/tag/arashazimi0032/Core)](https://github.com/arashazimi0032/Core/tags)  [![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/arashazimi0032/Core/dotnet-desktop.yml)](https://github.com/arashazimi0032/Core/actions/workflows/dotnet-desktop.yml)  [![GitHub last commit](https://img.shields.io/github/last-commit/arashazimi0032/Core)](https://github.com/arashazimi0032/Core/commits/master/)    [![GitHub commit activity](https://img.shields.io/github/commit-activity/m/arashazimi0032/Core)](https://github.com/arashazimi0032/Core/commits/master/)   [![GitHub Issues or Pull Requests](https://img.shields.io/github/issues/arashazimi0032/Core)](https://github.com/arashazimi0032/Core/issues) [![GitHub Issues or Pull Requests](https://img.shields.io/github/issues-pr/arashazimi0032/Core)](https://github.com/arashazimi0032/Core/pulls)  [![GitHub top language](https://img.shields.io/github/languages/top/arashazimi0032/Core)](https://github.com/arashazimi0032/Core)
---

This package is a Clean Architecture Base Template comprising all Baseic and Abstract and Contract types for **Messaging** between Microservices.

At this moment, we only use ``RabbitMQ`` for Messaging in this package.

This package is one of the *[Cleantemplate](https://www.nuget.org/packages/CleanTemplate)* side packages which provides the basic requirements for Calling Microservices using ``Messaging``.

The .Net version used in this project is net8.0

<p align="center" width="100%">
<img src="../icon.png" height="128" width="128"/>
</p>

# Contents

- [Dependencies](https://github.com/arashazimi0032/Core/tree/master/Core.Messaging#dependencies)
- [Installation](https://github.com/arashazimi0032/Core/tree/master/Core.Messaging#installation)
- [Usage](https://github.com/arashazimi0032/Core/tree/master/Core.Messaging#usage)
    * [Dependency Injection and Add Messaging to the projects](https://github.com/arashazimi0032/Core/tree/master/Core.Messaging#dependency-injection-and-add-messaging-to-the-projects)
    * [Create Contract](https://github.com/arashazimi0032/Core/tree/master/Core.Messaging#create-contract)
    * [Configuring RabbitMQ Settings](https://github.com/arashazimi0032/Core/tree/master/Core.Messaging#configuring-rabbitmq-settings)
    * [Create Publisher](https://github.com/arashazimi0032/Core/tree/master/Core.Messaging#create-publisher)
    * [Create Subscriber](https://github.com/arashazimi0032/Core/tree/master/Core.Messaging#create-subscriber)
    * [Using the Publisher for Publishing Message](https://github.com/arashazimi0032/Core/tree/master/Core.Messaging#using-the-publisher-for-publishing-message)

   
# Dependencies

## net8.0
- CleanTemplate (>= 7.5.0)
- RabbitMQ.Client (>= 6.8.1)

# Installation

.Net CLI

```
dotnet add package CleanTemplate.Messaging --version x.x.x
```

Package Manager

```
NuGet\Install-Package CleanTemplate.Messaging -Version x.x.x
```

Package Reference

```
<PackageReference Include="CleanTemplate.Messaging" Version="x.x.x" />
```

Paket CLI

```
paket add CleanTemplate.Messaging --version x.x.x
```

Script & Interactive

```
#r "nuget: CleanTemplate.Messaging, x.x.x"
```

Cake

```
// Install CleanTemplate.Messaging as a Cake Addin
#addin nuget:?package=CleanTemplate.Messaging&version=x.x.x

// Install CleanTemplate.Messaging as a Cake Tool
#tool nuget:?package=CleanTemplate.Messaging&version=x.x.x
```

# Usage

I tried to follow the basic format and structure of ``CleanTemplate`` here.

For use of this package we need three project:
1- A **Publisher** Web API project as First Microservice: Microservice1
2- A **Subscriber** Web API project as Second Microservice: Microservice2
3- A **Contract** ClassLibrary Project: Contracts

## Dependency Injection and Add Messaging to the projects
To use from ``CleanTemplate.Messaging`` in your project you should first register it in ``Program.cs`` of Publisher and Subscriber projects as below:

1 - Add below Extension Method (``AddCleanMessaging(Assembly assembly)``) to ``Program.cs`` of both **Publisher** and **Subscriber** projects:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCleanMessaging(assemblyOfYourPub/SubTypes)

var app = builder.Build();

app.Run();
```

2 - Also you should Add a ``DIModule`` To your **Contracts** Project and Register it inside of both **Publisher** and **Subscriber** Projects.

```csharp
// DIModule inside of your Contracts Project.

public class ContractDIModule : CleanBaseDIModule
{
}
```

```csharp
// Register ContractDIModule inside of ClientWebAPI Program.cs
builder.Services.AddCleanTemplateDIModule<ContractDIModule>();
```

3 - Also you should add below section in ``appsettings.json`` of both **Publisher** and **Subscriber** to configuring your RabbitMQ Connection Host.

```
"CleanRabbitMqHostSetting": {
"HostName": "localhost",
"UserName": "guest",
"Password": "guest"
}
```

## Create Contract

You should create a class inside of your **Contracts** project for your RabbitMQ Message from **Publisher** to **Subscriber** in which inherit from ``CleanBaseRabbitMqMessage``.
Also you should create a Setting type inside of your **Contracts** project in which inheriting from ``CleanBaseRabbitMqSetting`` for configuring your RabbitMQ Exchange, Queue, ExchangeType, ... .

```csharp
public class Service1To2Message : CleanBaseRabbitMqMessage
{
    // Some properties here
}
```

```csharp
public class Service1To2Settings : CleanBaseRabbitMqSetting
{
    public Service1To2Settings(IConfiguration configuration) : base(configuration)
    {
    }
}
```

## Configuring RabbitMQ Settings

You should configuring RabbitMQ settings for both **Publisher** and **Subscriber** in their ``appsettings.json`` files.
***Notable Point*** Just Note that your Section Name in ``appsettings.json`` should be equal to your Setting Class Name.

1 - For your **Publisher**:

```
"Service1To2Settings": {
    "QueueName": "MyQueueName",
    "ExchangeName": "MyExchangeName",
    "ExchangeType" : "Direct" or "Topic" or "Fanout" or "Headers",
    "RoutingKeies": [
        "rk1",
        "rk2",
        .
        .
        .
        "rkn"
    ]
  }
```

2 - For your **Subscriber**:

```
"Service1To2Settings": {
    "QueueName": "MyQueueName",
    "ExchangeName": "MyExchangeName",
    "ExchangeType" : "Direct" or "Topic" or "Fanout" or "Headers",
    "RoutingKeies": [
        "rk1",
        "rk2",
        .
        .
        .
        "rkn"
    ]
  }
```

## Create Publisher

Create a publishr class In your **Publisher** project in which inheriting from ``CleanBaseRabbitMqPublisher<TMessage, TSetting>``.

```csharp
public class Service1To2Publisher : CleanBaseRabbitMqPublisher<Service1To2Message, Service1To2Settings>
{
    public Service1To2Publisher(ICleanRabbitMqConnectionFactory connectionFactory, Service1To2Settings settings)
        : base(connectionFactory, settings)
    {
    }
}
```

## Create Subscriber

Create a subscriber class In your **Subscriber** project in which inheriting from ``CleanBaseRabbitMqSubscriber<TMessage, TSetting>``.

```csharp
public class Service1To2Subscriber : CleanBaseRabbitMqSubscriber<Service1To2Message, Service1To2Settings>
{
    public Service1To2Subscriber(ICleanRabbitMqConnectionFactory connectionFactory, Service1To2Settings settings) 
        : base(connectionFactory, settings)
    {
    }

    protected override async Task HandleAsync(Service1To2Message message)
    {
        await Console.Out.WriteLineAsync($"I Am Service2 Subscriber: Publisher Sent below data:\n{new { message.Name, message.Age }}");
    }
}
```


## Using the Publisher for Publishing Message

Finally to Sending Message from **Publisher** Microservice To **Subscriber** Microservice with RabbitMQ Message Broker, you should inject the publisher you created, into the controllers or services of the **Publisher**.

```csharp
[Route("api/[controller]")]
[ApiController]
public class Service1Controller : ControllerBase
{
    private readonly Service1To2Publisher _publisher;

    public Service1Controller(Service1To2Publisher publisher)
    {
        _publisher = publisher;
    }

    [HttpGet]
    public IActionResult SendMessage()
    {
        _publisher.PublishAsync(new Service1To2Message
        {
            // property initialization
        });
        return Ok();
    }
}
```
