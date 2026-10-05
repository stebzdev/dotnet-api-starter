using Starter.Application.Common.CQRS;
using Starter.Domain.ReportingEntities;

namespace Starter.Application.ReportingEntities.GetById;

public sealed record GetReportingEntityQuery(ReportingEntityId Id) : IQuery<GetReportingEntityResult>;
