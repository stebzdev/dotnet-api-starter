using Starter.Application.Common.CQRS;
using Starter.Application.ReportingPeriods.Create;
using Starter.Domain.ReportingEntities;
using Starter.Domain.ReportingPeriods;

namespace Starter.Application.ReportingPeriods.Create;

public sealed record CreateReportingPeriodCommand(
    ReportingEntityId ReportingEntityId,
    ReferencePeriod ReferencePeriod)
    : ICommand<CreateReportingPeriodResult>;
