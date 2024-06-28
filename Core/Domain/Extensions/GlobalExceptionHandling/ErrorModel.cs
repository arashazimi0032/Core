using Core.Domain.Exceptions;

namespace Core.Domain.Extensions.GlobalExceptionHandling;
internal class ErrorModel
{
    internal ErrorModel()
    {
        RequestHeaders = new Dictionary<string, string>();
    }
    public DateTime ErrorTime { get; set; }
    public string ControllerName { get; set; } = string.Empty;
    public string ActionName { get; set; } = string.Empty;
    public string HttpMethod { get; set; } = string.Empty;
    public string ExceptionType { get; set; } = string.Empty;
    public ExceptionMessage CleanExceptionMessage { get; set; }
    public string UnHandledExceptionMessage { get; set; }
    public string StackTrace { get; set; } = string.Empty;
    public string UserIpAddress { get; set; } = string.Empty;
    public string RequestUrl { get; set; } = string.Empty;
    public string RequestBody { get; set; } = string.Empty;
    public Dictionary<string, string> RequestHeaders { get; set; }
}
