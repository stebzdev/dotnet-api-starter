using Starter.Application.Common.CQRS;
using Starter.Domain.ReportingEntities;

namespace Starter.Application.ReportingEntities.Create;

public sealed record CreateReportingEntityCommand(
    string Name,
    ReportingEntityCode Code)
    : ICommand<CreateReportingEntityResult>;
