using Core.Domain.DIModules;
using Core.Domain.Extensions.Endpoint;
using Core.Domain.Extensions.GlobalExceptionHandling;
using Core.Domain.Extensions.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace Core.Domain.Extensions.CleanTemplate;

public static class CleanTemplateExtensions
{
    public static IServiceCollection AddCleanTemplateDIModule<TDIModule>(this IServiceCollection services)
        where TDIModule : CleanBaseDIModule
    {
        var assembly = typeof(TDIModule).Assembly;

        services.AddCleanTemplate(assembly);

        return services;
    }
    public static IServiceCollection AddCleanTemplate(this IServiceCollection services)
    {
        var assembly = Assembly.GetCallingAssembly();

        services.AddCleanTemplate(assembly);

        return services;
    }

    public static WebApplicationBuilder AddCleanLogger(
        this WebApplicationBuilder builder, 
        string elasticSearchUri, 
        string? elasticSearchUsername = null,
        string? elasticSearchPassword = null)
    {
        string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        ElasticsearchSinkOptions elasticsearchSinkOptions = new ElasticsearchSinkOptions(new Uri(elasticSearchUri))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"{Assembly.GetCallingAssembly().GetName().Name?.ToLower()}-{environment?.ToLower()}-{DateTime.Now:yyyy-MM-dd}" ?? "",
            ModifyConnectionSettings = x => x
                                            .BasicAuthentication(elasticSearchUsername ?? "elastic", elasticSearchPassword ?? "")
                                            .ServerCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true)
        };

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Elasticsearch(elasticsearchSinkOptions)
        .Enrich.WithProperty("Environment", environment)
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog();

        return builder;
    }

    public static IApplicationBuilder UseCleanTemplate(this IApplicationBuilder app)
    {
        var assembly = Assembly.GetCallingAssembly();

        app.UseCleanGlobalExceptionHandling();

        app.UseRouting();

        app.UseCleanMiddlewares(assembly);

        app.UseCleanEndpoints(assembly);

        return app;
    }
}
