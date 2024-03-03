using Core.Domain.BaseModels;

namespace Core.Domain.Results;

public sealed record Error : CleanBaseValueObject
{
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }
    public string Message { get; }

    public static implicit operator string(Error error) => error?.Code ?? string.Empty;

    public static Error None => new Error(string.Empty, string.Empty);
}