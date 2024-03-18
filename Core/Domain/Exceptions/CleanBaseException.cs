using Newtonsoft.Json;
using System.Net;

namespace Core.Domain.Exceptions;

public abstract class CleanBaseException : SystemException, ICleanBaseException
{
    public abstract string DefaultMessage { get; }
    public virtual HttpStatusCode HttpStatusResponseType => HttpStatusCode.InternalServerError;
    public string HttpStatusResponseCode => Convert.ToInt32(HttpStatusResponseType).ToString();

    private readonly bool _hasMessage = true;
    protected CleanBaseException()
    {
        _hasMessage = false;
    }
    protected CleanBaseException(string? message)
        : base(message)
    {
        _hasMessage = true;
    }
    protected CleanBaseException(string? message, Exception? innerException)
        : base(message, innerException)
    {
        _hasMessage = true;
    }
    public override string Message
    {
        get
        {
            if (!_hasMessage)
            {
                return DefaultMessage;
            }
            return base.Message;
        }
    }

    public virtual ExceptionMessage GetMessage()
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
            return exceptionMessage;
        }

        if (InnerException is ICleanBaseException cleanInnerException)
        {
            exceptionMessage.CleanInnerException = cleanInnerException.GetMessage();
        }
        else
        {
            exceptionMessage.UnHandledInnerException = InnerException.ToString();
        }

        return exceptionMessage;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(GetMessage());
    }
}

public abstract class CleanBaseException<TCode> : CleanBaseException, ICleanBaseException
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

    public override ExceptionMessage GetMessage()
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
            return exceptionMessage;
        }

        if (InnerException is ICleanBaseException cleanInnerException)
        {
            exceptionMessage.CleanInnerException = cleanInnerException.GetMessage();
        }
        else
        {
            exceptionMessage.UnHandledInnerException = InnerException.ToString();
        }

        return exceptionMessage;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(GetMessage());
    }
}