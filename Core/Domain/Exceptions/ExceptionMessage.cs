namespace Core.Domain.Exceptions;

internal class ExceptionMessage
{
    public string? Title {  get; set; }
    public string? HttpStatusResponseType { get; set; }
    public string? HttpStatusResponseCode { get; set; }
    public string? ExceptionType { get; set; }
    public string? ResourceKey { get; set; }
    public string? Message { get; set; }
    public ExceptionMessage? CleanInnerException { get; set; }
    public string? UnHandledInnerException { get; set; }
}
