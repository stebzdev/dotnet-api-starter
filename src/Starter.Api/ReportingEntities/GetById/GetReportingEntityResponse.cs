namespace Starter.Api.ReportingEntities.GetById;

public sealed record GetReportingEntityResponse(
    Guid Id,
    string Name,
    string Code);
