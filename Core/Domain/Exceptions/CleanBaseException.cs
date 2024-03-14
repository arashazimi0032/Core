using Core.Application.ServiceLifeTimes;
using System.Net;

namespace Core.Domain.Exceptions;

public abstract class CleanBaseException : SystemException, ICleanBaseIgnore
{
    public virtual HttpStatusCode HttpStatusResponseType => HttpStatusCode.InternalServerError;
    public string HttpStatusResponseCode => Convert.ToInt32(HttpStatusResponseType).ToString();

    protected CleanBaseException()
    {
    }
    protected CleanBaseException(string? message)
        : base(message)
    {
    }
    protected CleanBaseException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public override string ToString()
    {
        string fullMessage = InnerException is null ?
            $"Message: {Message}" :
            $"Message: {Message} \n\nInnerException: \n{InnerException}";

        return $"Clean Template Exception: \nHttpStatusResponseType: {HttpStatusResponseType} \nHttpStatusResponseCode: {HttpStatusResponseCode} \n{fullMessage}";
    }
}

public abstract class CleanBaseException<TCode> : CleanBaseException
    where TCode : Enum
{
    public abstract TCode ExceptionType { get; }

    public virtual string ResourceKey => Convert.ToInt32(ExceptionType).ToString();

    protected CleanBaseException()
    {
    }
    protected CleanBaseException(string? message) 
        : base(message)
    {
    }
    protected CleanBaseException(string? message, Exception? innerException) 
        : base(message, innerException)
    {
    }

    public override string ToString()
    {
        string fullMessage = InnerException is null ?
            $"Message: {Message}" :
            $"Message: {Message} \n\nInnerException: \n{InnerException}";

        return $"CleanTemplateException: \nHttpStatusResponseType: {HttpStatusResponseType} \nHttpStatusResponseCode: {HttpStatusResponseCode} \nExceptionType: {ExceptionType} \nResourceKey: {ResourceKey} \n{fullMessage}";
    }
}
