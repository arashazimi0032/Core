using Core.Domain.Enums;

namespace Core.Domain.Exceptions;

internal class CleanTemplateInternalException : CleanBaseException<CleanBaseExceptionCode>
{
    private readonly CleanBaseExceptionCode _internalExceptionCode = CleanBaseExceptionCode.Exception;

    public override string DefaultMessage => "CleanTemplateInternalException";

    internal CleanTemplateInternalException()
    {
    }

    internal CleanTemplateInternalException(CleanBaseExceptionCode exceptionCode)
    {
        _internalExceptionCode = exceptionCode;
    }

    internal CleanTemplateInternalException(string message)
        : base(message)
    {
    }

    internal CleanTemplateInternalException(string message, CleanBaseExceptionCode exceptionCode)
        : base(message)
    {
        _internalExceptionCode = exceptionCode;
    }

    internal CleanTemplateInternalException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
    
    internal CleanTemplateInternalException(string message, CleanBaseExceptionCode exceptionCode, Exception innerException)
        : base(message, innerException)
    {
        _internalExceptionCode = exceptionCode;
    }

    public override CleanBaseExceptionCode ExceptionType => _internalExceptionCode;
}
