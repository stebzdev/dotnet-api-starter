using Starter.Domain.ReportingEntities;
using Starter.Domain.ReportingPeriods;
using Microsoft.Extensions.Logging;

namespace Starter.Application.ReportingPeriods.Create;

internal static partial class CreateReportingPeriodLog
{
    [LoggerMessage( Level = LogLevel.Warning, Message = "Reporting entity {ReportingEntityId} was not found while creating a reporting period")]
    public static partial void ReportingEntityNotFound(ILogger logger, Guid reportingEntityId);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Reporting period already exists for reporting entity {ReportingEntityId} and reference period {ReferencePeriodYear}-{ReferencePeriodMonth:D2}")]
    public static partial void AlreadyExists(ILogger logger, Guid reportingEntityId, int referencePeriodYear, int referencePeriodMonth);

    [LoggerMessage(Level = LogLevel.Information, Message = "Reporting period {ReportingPeriodId} created for reporting entity {ReportingEntityId} and reference period {ReferencePeriodYear}-{ReferencePeriodMonth:D2}")]
    public static partial void Created(ILogger logger, Guid reportingPeriodId, Guid reportingEntityId, int referencePeriodYear, int referencePeriodMonth);
}
