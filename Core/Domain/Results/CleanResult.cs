namespace Core.Domain.Results;

public class CleanResult
{
    protected CleanResult(bool isSuccess, CleanError error)
    {
        if (isSuccess && error != CleanError.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == CleanError.None)
        {
            throw new InvalidOperationException();
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