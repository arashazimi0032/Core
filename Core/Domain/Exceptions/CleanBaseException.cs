using Core.Application.ServiceLifeTimes;
using System.Net;

namespace Core.Domain.Exceptions;

public abstract class CleanBaseException<TCode> : SystemException, ICleanBaseIgnore
    where TCode : Enum
{
    public abstract TCode ExceptionCode { get; }

    public virtual HttpStatusCode HttpStatusCode => HttpStatusCode.InternalServerError;

    public virtual string ResourceKey => Convert.ToInt32(ExceptionCode).ToString();

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
            $"Message: {Message} \n InnerExceptionMessage: {InnerException.Message}";

        return $"Clean Template Exception: \n {fullMessage}";
    }
}
