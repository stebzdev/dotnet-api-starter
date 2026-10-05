using Starter.Application.Common.CQRS;
using Starter.Application.Common.Persistence;
using Starter.Application.Common.Results;
using Starter.Domain.ReportingPeriods;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Starter.Application.ReportingPeriods.Create;

public sealed class CreateReportingPeriodHandler(
    StarterDbContext dbContext, 
    ILogger<CreateReportingPeriodHandler> logger) : ICommandHandler<CreateReportingPeriodCommand, CreateReportingPeriodResult>
{
    public async Task<Result<CreateReportingPeriodResult>> HandleAsync(CreateReportingPeriodCommand command, CancellationToken cancellationToken)
    {
        var reportingEntityExists = await dbContext.ReportingEntities.AnyAsync(
            x => x.Id == command.ReportingEntityId,
            cancellationToken);

        if (!reportingEntityExists)
        {
            CreateReportingPeriodLog.ReportingEntityNotFound(logger, command.ReportingEntityId.Value);

            return Result<CreateReportingPeriodResult>.Failure(CreateReportingPeriodErrors.ReportingEntityNotFound);
        }

        var alreadyExists = await dbContext.ReportingPeriods.AnyAsync(
            x => x.ReportingEntityId == command.ReportingEntityId &&
                 x.ReferencePeriod.Year == command.ReferencePeriod.Year &&
                 x.ReferencePeriod.Month == command.ReferencePeriod.Month,
            cancellationToken);

        if (alreadyExists)
        {
            CreateReportingPeriodLog.AlreadyExists(logger, command.ReportingEntityId.Value, command.ReferencePeriod.Year, command.ReferencePeriod.Month);

            return Result<CreateReportingPeriodResult>.Failure(CreateReportingPeriodErrors.AlreadyExists);
        }

        var reportingPeriod = ReportingPeriod.Create(
            command.ReportingEntityId,
            command.ReferencePeriod);

        dbContext.ReportingPeriods.Add(reportingPeriod);
        await dbContext.SaveChangesAsync(cancellationToken);

        CreateReportingPeriodLog.Created(logger, reportingPeriod.Id.Value, reportingPeriod.ReportingEntityId.Value, reportingPeriod.ReferencePeriod.Year, reportingPeriod.ReferencePeriod.Month);

        var response = new CreateReportingPeriodResult(reportingPeriod.Id);
        return Result<CreateReportingPeriodResult>.Success(response);
    }
}
