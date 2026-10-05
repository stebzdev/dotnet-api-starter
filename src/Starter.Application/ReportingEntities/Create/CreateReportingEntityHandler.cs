using Starter.Application.Common.CQRS;
using Starter.Application.Common.Persistence;
using Starter.Application.Common.Results;
using Starter.Domain.ReportingEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Starter.Application.ReportingEntities.Create;

public sealed class CreateReportingEntityHandler(
    StarterDbContext dbContext,
    ILogger<CreateReportingEntityHandler> logger) : ICommandHandler<CreateReportingEntityCommand, CreateReportingEntityResult>
{
    public async Task<Result<CreateReportingEntityResult>> HandleAsync(CreateReportingEntityCommand command, CancellationToken cancellationToken)
    {
        var alreadyExists = await dbContext.ReportingEntities.AnyAsync(
            x => x.Code == command.Code,
            cancellationToken);

        if (alreadyExists)
        {
            CreateReportingEntityLog.AlreadyExists(logger, command.Code.Value);

            return Result<CreateReportingEntityResult>.Failure(ReportingEntityErrors.AlreadyExists);
        }

        var reportingEntity = ReportingEntity.Create(command.Name, command.Code);

        dbContext.ReportingEntities.Add(reportingEntity);

        await dbContext.SaveChangesAsync(cancellationToken);

        CreateReportingEntityLog.Created(logger, reportingEntity.Id.Value, reportingEntity.Code.Value);

        var response = new CreateReportingEntityResult(reportingEntity.Id);

        return Result<CreateReportingEntityResult>.Success(response);
    }
}
