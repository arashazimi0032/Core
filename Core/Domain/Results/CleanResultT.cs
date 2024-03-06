namespace Core.Domain.Results;

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
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");
}