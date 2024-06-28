using Core.Domain.Enums;
using Core.Domain.Exceptions;

namespace Core.Messaging.Exceptions;

internal class CleanTemplateMessagingInternalException : CleanBaseException<CleanBaseExceptionCode>
{
    private readonly CleanBaseExceptionCode _internalExceptionCode = CleanBaseExceptionCode.Exception;

    public override string DefaultMessage => "CleanTemplateMessagingInternalException";

    internal CleanTemplateMessagingInternalException()
    {
    }

    internal CleanTemplateMessagingInternalException(CleanBaseExceptionCode exceptionCode)
    {
        _internalExceptionCode = exceptionCode;
    }

    internal CleanTemplateMessagingInternalException(string message)
        : base(message)
    {
    }

    internal CleanTemplateMessagingInternalException(string message, CleanBaseExceptionCode exceptionCode)
        : base(message)
    {
        _internalExceptionCode = exceptionCode;
    }

    internal CleanTemplateMessagingInternalException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    internal CleanTemplateMessagingInternalException(string message, CleanBaseExceptionCode exceptionCode, Exception innerException)
        : base(message, innerException)
    {
        _internalExceptionCode = exceptionCode;
    }

    public override CleanBaseExceptionCode ExceptionType => _internalExceptionCode;
}
