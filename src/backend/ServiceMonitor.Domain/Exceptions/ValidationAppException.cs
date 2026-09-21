namespace ServiceMonitor.Domain.Exceptions;

public class ValidationAppException(IReadOnlyDictionary<string, string[]> errors)
    : Exception("One or more validation errors have occurred")
{
    public IReadOnlyDictionary<string, string[]> Errors { get; } = errors;
}
