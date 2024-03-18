using Core.Domain.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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

                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorModel).ToString());
            });
        });
    }
}
