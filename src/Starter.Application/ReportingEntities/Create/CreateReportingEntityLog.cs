using Starter.Domain.ReportingEntities;
using Microsoft.Extensions.Logging;

namespace Starter.Application.ReportingEntities.Create;

internal static partial class CreateReportingEntityLog
{
    [LoggerMessage(Level = LogLevel.Warning, Message = "Reporting entity with code {ReportingEntityCode} already exists")]
    public static partial void AlreadyExists(ILogger logger, string reportingEntityCode);

    [LoggerMessage(Level = LogLevel.Information, Message = "Reporting entity {ReportingEntityId} created with code {ReportingEntityCode}")]
    public static partial void Created(ILogger logger, Guid reportingEntityId, string reportingEntityCode);
}
