using Core.Domain.BaseModels;

namespace Core.Domain.Results;

public sealed record CleanError : CleanBaseValueObject
{
    public CleanError(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }
    public string Message { get; }

    public static implicit operator string(CleanError error) => error?.Code ?? string.Empty;

    public static CleanError None => new CleanError(string.Empty, string.Empty);
}