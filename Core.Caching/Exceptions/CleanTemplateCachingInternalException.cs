using Core.Domain.Enums;
using Core.Domain.Exceptions;

namespace Core.Caching.Exceptions;

internal class CleanTemplateCachingInternalException : CleanBaseException<CleanBaseExceptionCode>
{
    private readonly CleanBaseExceptionCode _internalExceptionCode = CleanBaseExceptionCode.Exception;

    public override string DefaultMessage => "CleanTemplateCachingInternalException";

    internal CleanTemplateCachingInternalException()
    {
    }

    internal CleanTemplateCachingInternalException(CleanBaseExceptionCode exceptionCode)
    {
        _internalExceptionCode = exceptionCode;
    }

    internal CleanTemplateCachingInternalException(string? message)
        : base(message)
    {
    }

    internal CleanTemplateCachingInternalException(string? message, CleanBaseExceptionCode exceptionCode)
        : base(message)
    {
        _internalExceptionCode = exceptionCode;
    }

    internal CleanTemplateCachingInternalException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    internal CleanTemplateCachingInternalException(string? message, CleanBaseExceptionCode exceptionCode, Exception? innerException)
        : base(message, innerException)
    {
        _internalExceptionCode = exceptionCode;
    }

    public override CleanBaseExceptionCode ExceptionType => _internalExceptionCode;
}
