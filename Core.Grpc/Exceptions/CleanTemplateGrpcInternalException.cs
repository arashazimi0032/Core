using Core.Domain.Enums;
using Core.Domain.Exceptions;

namespace Core.Grpc.Exceptions;

internal class CleanTemplateGrpcInternalException : CleanBaseException<CleanBaseExceptionCode>
{
    private readonly CleanBaseExceptionCode _internalExceptionCode = CleanBaseExceptionCode.Exception;

    public override string DefaultMessage => "CleanTemplateGrpcInternalException";

    internal CleanTemplateGrpcInternalException()
    {
    }

    internal CleanTemplateGrpcInternalException(CleanBaseExceptionCode exceptionCode)
    {
        _internalExceptionCode = exceptionCode;
    }

    internal CleanTemplateGrpcInternalException(string message)
        : base(message)
    {
    }

    internal CleanTemplateGrpcInternalException(string message, CleanBaseExceptionCode exceptionCode)
        : base(message)
    {
        _internalExceptionCode = exceptionCode;
    }

    internal CleanTemplateGrpcInternalException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    internal CleanTemplateGrpcInternalException(string message, CleanBaseExceptionCode exceptionCode, Exception innerException)
        : base(message, innerException)
    {
        _internalExceptionCode = exceptionCode;
    }

    public override CleanBaseExceptionCode ExceptionType => _internalExceptionCode;
}
