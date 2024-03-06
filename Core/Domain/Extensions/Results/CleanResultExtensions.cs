using Core.Domain.Results;

namespace Core.Domain.Extensions.Results;

public static class CleanResultExtensions
{
    public static CleanResult<T> Ensure<T>(this CleanResult<T> result, Func<T, bool> predicate, CleanError error)
    {
        if (result.IsFailure)
        {
            return result;
        }

        return result.IsSuccess && predicate(result.Value) ? result : CleanResult.Failure<T>(error);
    }

    public static CleanResult<TOut> Map<TIn, TOut>(this CleanResult<TIn> result, Func<TIn, TOut> func) =>
        result.IsSuccess ? func(result.Value) : CleanResult.Failure<TOut>(result.Error);

    public static async Task<CleanResult> Bind<TIn>(this CleanResult<TIn> result, Func<TIn, Task<CleanResult>> func) =>
        result.IsSuccess ? await func(result.Value) : CleanResult.Failure(result.Error);

    public static async Task<CleanResult<TOut>> Bind<TIn, TOut>(this CleanResult<TIn> result, Func<TIn, Task<CleanResult<TOut>>> func) =>
        result.IsSuccess ? await func(result.Value) : CleanResult.Failure<TOut>(result.Error);

    public static async Task<T> Match<T>(this Task<CleanResult> resultTask, Func<T> onSuccess, Func<CleanError, T> onFailure)
    {
        CleanResult result = await resultTask;

        return result.IsSuccess ? onSuccess() : onFailure(result.Error);
    }

    public static async Task<TOut> Match<TIn, TOut>(
        this Task<CleanResult<TIn>> resultTask,
        Func<TIn, TOut> onSuccess,
        Func<CleanError, TOut> onFailure)
    {
        CleanResult<TIn> result = await resultTask;

        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
    }
}