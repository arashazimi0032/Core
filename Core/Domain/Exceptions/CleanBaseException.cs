using Core.Application.ServiceLifeTimes;
using Newtonsoft.Json;
using System.Net;

namespace Core.Domain.Exceptions;

public abstract class CleanBaseException : SystemException, ICleanBaseIgnore
{
    public abstract string DefaultMessage { get; }
    public virtual HttpStatusCode HttpStatusResponseType => HttpStatusCode.InternalServerError;
    public string HttpStatusResponseCode => Convert.ToInt32(HttpStatusResponseType).ToString();
    protected CleanBaseException()
    {
        base.Data["DefaultMessage"] = DefaultMessage;
    }
    protected CleanBaseException(string? message)
        : base(message)
    {
    }
    protected CleanBaseException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
    public override string Message
    {
        get
        {
            if (base.Data.Contains("DefaultMessage"))
            {
                return base.Data["DefaultMessage"]?.ToString() ?? "";
            }
            return base.Message;
        }
    }

    public override string ToString()
    {
        var exceptionMessage = new ExceptionMessage()
        {
            Title = "CleanTemplateException",
            HttpStatusResponseType = HttpStatusResponseType.ToString(),
            HttpStatusResponseCode = HttpStatusResponseCode,
            Message = Message,
        };

        if (InnerException is null)
        {
            return JsonConvert.SerializeObject(exceptionMessage);
        }

        if (InnerException.GetType().IsAssignableTo(typeof(CleanBaseException)))
        {
            exceptionMessage.CleanInnerException = JsonConvert.DeserializeObject<ExceptionMessage>(InnerException.ToString());
        }
        else
        {
            exceptionMessage.UnHandledInnerException = InnerException.ToString();
        }

        return JsonConvert.SerializeObject(exceptionMessage);
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
        var exceptionMessage = new ExceptionMessage()
        {
            Title = "CleanTemplateException",
            HttpStatusResponseType = HttpStatusResponseType.ToString(),
            HttpStatusResponseCode = HttpStatusResponseCode,
            ExceptionType = ExceptionType.ToString(),
            ResourceKey = ResourceKey,
            Message = Message,
        };

        if (InnerException is null)
        {
            return JsonConvert.SerializeObject(exceptionMessage);
        }

        if (InnerException.GetType().IsAssignableTo(typeof(CleanBaseException)))
        {
            exceptionMessage.CleanInnerException = JsonConvert.DeserializeObject<ExceptionMessage>(InnerException.ToString());
        }
        else
        {
            exceptionMessage.UnHandledInnerException = InnerException.ToString();
        }

        return JsonConvert.SerializeObject(exceptionMessage);
    }
}