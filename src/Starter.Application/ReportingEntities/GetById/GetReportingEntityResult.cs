using Starter.Domain.ReportingEntities;

namespace Starter.Application.ReportingEntities.GetById;

public sealed record GetReportingEntityResult(ReportingEntityId Id, string Name, ReportingEntityCode Code);
