using Core.Domain.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System.Net;

namespace Core.Domain.Extensions.GlobalExceptionHandling;

internal static class CleanGlobalExceptionExtensions
{
    internal static void UseCleanGlobalExceptionHandling(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(error =>
        {
            error.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature is null) return;

                var exception = contextFeature.Error;

                if (exception is null) return;

                var errorModel = new ErrorModel
                {
                    ErrorTime = DateTime.Now,
                    ControllerName = $"{context.Request.Scheme}://{context.Request.Host}{string.Join("/", context.Request.Path.ToString().Split("/")[..^1])}Controller",
                    ActionName = string.Join("/", context.Request.Path.ToString().Split("/")[^1]),
                    ExceptionType = exception.GetType().FullName,
                    StackTrace = exception.StackTrace,
                    RequestUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}",
                    HttpMethod = context.Request.Method,
                    RequestBody = await new StreamReader(context.Request.Body).ReadToEndAsync(),
                    RequestHeaders = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                    UserIpAddress = context.Connection.RemoteIpAddress?.ToString(),
                };

                if (exception is ICleanBaseException cleanBaseException)
                {
                    context.Response.StatusCode = (int)cleanBaseException.HttpStatusResponseType;
                    errorModel.CleanExceptionMessage = cleanBaseException.GetMessage();
                }
                else
                {
                    errorModel.UnHandledExceptionMessage = exception.Message;
                }

                Log.Logger.Error(exception, CreateMessageTemplate(errorModel));

                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorModel));
            });
        });
    }

    #region Private
    private static string CreateMessageTemplate(ErrorModel errorModel)
    {
        var toReturn = "{\n" +
            $"\tErrorTime: {errorModel.ErrorTime}\n" +
            $"\tControllerName: {errorModel.ControllerName}\n" +
            $"\tActionName: {errorModel.ActionName}\n" +
            $"\tHttpMethod: {errorModel.HttpMethod}\n" +
            $"\tExceptionType: {errorModel.ExceptionType}\n" +
            "\tCleanExceptionMessage: {\n" +
            $"{GetNestedTypes(errorModel.CleanExceptionMessage)}\n" +
            "\t}\n" +
            $"\tUnHandledExceptionMessage: {errorModel.UnHandledExceptionMessage}\n" +
            $"\tUserIpAddress: {errorModel.UserIpAddress}\n" +
            $"\tRequestUrl: {errorModel.RequestUrl}\n" +
            $"\tRequestBody: {errorModel.RequestBody}\n" +
            "\tRequestHeaders: {\n" +
            $"{GetDictionaryString(errorModel.RequestHeaders)}\n" +
            "\t}\n" +
            "}";

        return toReturn;
    }

    private static string GetNestedTypes(ExceptionMessage? exceptionMessage, int nestedLevel = 1)
    {
        var toReturn = "";
        var tab = string.Concat(Enumerable.Repeat("\t", nestedLevel));
        if (exceptionMessage is not null)
        {
            foreach (var property in exceptionMessage.GetType().GetProperties())
            {
                if (property.PropertyType.Equals(typeof(ExceptionMessage)))
                {
                    toReturn += $"\t{tab}{property.Name}: " + "{\n";
                    toReturn += $"{GetNestedTypes((ExceptionMessage?)property.GetValue(exceptionMessage), nestedLevel + 1)}" +
                        $"\t{tab}" + "}\n";
                }
                else
                {
                    toReturn += $"\t{tab}{property.Name}: {property.GetValue(exceptionMessage)}\n";
                }
            }
        }
        else
        {
            toReturn += $"\t{tab}null\n";
        }

        return toReturn;
    }

    private static string GetDictionaryString(Dictionary<string, string> dictionary)
    {
        var toReturn = "";

        foreach (var key in dictionary.Keys)
        {
            toReturn += $"\t\t{key}: {dictionary[key]}\n";
        }
        return toReturn;
    } 
    #endregion
}
