using Core.Domain.Enums;
using Core.Domain.Exceptions;

namespace Core.Domain.Results;

public class CleanResult
{
    protected CleanResult(bool isSuccess, CleanError error)
    {
        if (isSuccess && error != CleanError.None)
        {
            throw new CleanTemplateInternalException(
                "A success Result should has None Error.",
                CleanBaseExceptionCode.InvalidOperationException,
                new CleanTemplateInternalException(
                    "CleanResult class could not be success with non-None error at the same time.",
                    CleanBaseExceptionCode.CleanResultException));
        }

        if (!isSuccess && error == CleanError.None)
        {
            throw new CleanTemplateInternalException(
                "A failure Result should not has None Error.",
                CleanBaseExceptionCode.InvalidOperationException,
                new CleanTemplateInternalException(
                    "CleanResult class could not be failure with None error at the same time.",
                    CleanBaseExceptionCode.CleanResultException));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;
    public CleanError Error { get; }

    public static CleanResult Success() => new CleanResult(true, CleanError.None);

    public static CleanResult<TValue> Success<TValue>(TValue value) => new CleanResult<TValue>(value, true, CleanError.None);

    public static CleanResult<TValue> Create<TValue>(TValue value, CleanError error)
        where TValue : class
        => value is null ? Failure<TValue>(error) : Success(value);

    public static CleanResult Failure(CleanError error) => new CleanResult(false, error);

    public static CleanResult<TValue> Failure<TValue>(CleanError error) => new CleanResult<TValue>(default!, false, error);

    public static CleanResult FirstFailureOrSuccess(params CleanResult[] results)
    {
        foreach (CleanResult result in results)
        {
            if (result.IsFailure)
            {
                return result;
            }
        }

        return Success();
    }
}

public class CleanResult<TValue> : CleanResult
{

    private readonly TValue _value;
    protected internal CleanResult(TValue value, bool isSuccess, CleanError error) : base(isSuccess, error)
    {
        _value = value;
    }

    public static implicit operator CleanResult<TValue>(TValue value) => Success(value);

    public TValue Value => IsSuccess
        ? _value
        : throw new CleanTemplateInternalException(
            "The value of a failure result can not be accessed.",
            CleanBaseExceptionCode.InvalidOperationException,
            new CleanTemplateInternalException(
                "CleanResult class could not return the value of a failure result!",
                CleanBaseExceptionCode.CleanResultException));
}
