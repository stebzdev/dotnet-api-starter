namespace Starter.Api.ReportingPeriods.Create;

public sealed record CreateReportingPeriodRequest(
    Guid ReportingEntityId,
    int ReferenceYear,
    int ReferenceMonth);
