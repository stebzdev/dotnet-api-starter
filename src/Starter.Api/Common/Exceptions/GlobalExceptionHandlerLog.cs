namespace Starter.Api.Common.Exceptions;

internal static class GlobalExceptionHandlerLog
{
    private static readonly Action<ILogger, string, Exception?> DomainValidationErrorMessage =
        LoggerMessage.Define<string>(LogLevel.Warning, new EventId(1001, nameof(LogDomainValidationError)), "Domain validation error. TraceId: {TraceId}");

    private static readonly Action<ILogger, string, Exception?> UnhandledExceptionMessage =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(1002, nameof(LogUnhandledException)), "Unhandled exception. TraceId: {TraceId}");

    internal static void LogDomainValidationError(ILogger logger, Exception exception, string traceId)
    {
        DomainValidationErrorMessage(logger, traceId, exception);
    }

    internal static void LogUnhandledException(ILogger logger, Exception exception, string traceId)
    {
        UnhandledExceptionMessage(logger, traceId, exception);
    }
}
