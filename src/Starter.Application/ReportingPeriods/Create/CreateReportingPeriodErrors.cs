using Starter.Application.Common.Results;

namespace Starter.Application.ReportingPeriods.Create;

public static class CreateReportingPeriodErrors
{
    public static readonly Error AlreadyExists = new(
        "ReportingPeriod.AlreadyExists",
        "A reporting period already exists for the specified entity and period.",
        ErrorType.Conflict);

    public static readonly Error ReportingEntityNotFound = new(
        "ReportingPeriod.ReportingEntityNotFound",
        "The specified reporting entity does not exist.",
        ErrorType.NotFound);
}
