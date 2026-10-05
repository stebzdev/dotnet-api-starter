using Starter.Application.Common.CQRS;
using Starter.Application.Common.Persistence;
using Starter.Application.Common.Results;
using Starter.Domain.SampleItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Starter.Application.SampleItems.Create;

public sealed class CreateSampleItemHandler(
    StarterDbContext dbContext,
    ILogger<CreateSampleItemHandler> logger) : ICommandHandler<CreateSampleItemCommand, CreateSampleItemResult>
{
    public async Task<Result<CreateSampleItemResult>> HandleAsync(CreateSampleItemCommand command, CancellationToken cancellationToken)
    {
        var alreadyExists = await dbContext.SampleItems.AnyAsync(
            x => x.Code == command.Code,
            cancellationToken);

        if (alreadyExists)
        {
            CreateSampleItemLog.AlreadyExists(logger, command.Code.Value);

            return Result<CreateSampleItemResult>.Failure(SampleItemErrors.AlreadyExists);
        }

        var sampleEntity = SampleItem.Create(command.Name, command.Code);

        dbContext.SampleItems.Add(sampleEntity);

        await dbContext.SaveChangesAsync(cancellationToken);

        CreateSampleItemLog.Created(logger, sampleEntity.Id.Value, sampleEntity.Code.Value);

        var response = new CreateSampleItemResult(sampleEntity.Id);

        return Result<CreateSampleItemResult>.Success(response);
    }
}
