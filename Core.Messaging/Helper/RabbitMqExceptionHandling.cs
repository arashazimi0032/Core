using Core.Domain.Exceptions;
using Core.Domain.Extensions.GlobalExceptionHandling;
using Core.Messaging.Messages;
using Core.Messaging.Settings;
using Newtonsoft.Json;
using Serilog;

namespace Core.Messaging.Helper;

internal static class RabbitMqExceptionHandling
{
    internal static void HandleException<TMessage, TSetting>(Exception ex, string subscriberName, TSetting setting, TMessage message = null)
        where TMessage : CleanBaseRabbitMqMessage
        where TSetting : CleanBaseRabbitMqSetting
    {
        var errorModel = new RabbitMqErrorModel
        {
            StackTrace = ex.StackTrace,
            ExceptionType = ex.GetType().FullName,
            RabbitMqMessage = JsonConvert.SerializeObject(message),
            RabbitMqSettings = JsonConvert.SerializeObject(setting),
            SubscriberName = subscriberName
        };

        if (ex is ICleanBaseException cleanBaseException)
        {
            errorModel.CleanExceptionMessage = cleanBaseException.GetMessage();
        }
        else
        {
            errorModel.UnHandledExceptionMessage = ex.Message;
        }

        Log.Logger.Error(ex, CreateMessageTemplate(errorModel));
    }

    private static string CreateMessageTemplate(RabbitMqErrorModel errorModel)
    {
        var toReturn = "{\n" +
            $"\tExceptionType: {errorModel.ExceptionType}\n" +
            "\tCleanExceptionMessage: {\n" +
            $"{MessageTemplateHelper.GetNestedTypes(errorModel.CleanExceptionMessage)}\n" +
            "\t}\n" +
            $"\tUnHandledExceptionMessage: {errorModel.UnHandledExceptionMessage}\n" +
            $"\tSubscriberName: {errorModel.SubscriberName}\n" +
            $"\tRabbitMqMessage: {errorModel.RabbitMqMessage}\n" +
            $"\tRabbitMqSettings: {errorModel.RabbitMqSettings}\n" +
            "}";

        return toReturn;
    }
}
