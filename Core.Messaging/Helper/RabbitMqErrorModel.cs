using Core.Domain.Exceptions;

namespace Core.Messaging.Helper;

internal class RabbitMqErrorModel
{
    public ExceptionMessage CleanExceptionMessage { get; set; }
    public string UnHandledExceptionMessage { get; set; }
    public string StackTrace { get; set; } = string.Empty;
    public string ExceptionType { get; set; } = string.Empty;
    public string RabbitMqMessage { get; set; }
    public string RabbitMqSettings { get; set; }
    public string SubscriberName { get; set; }
}
