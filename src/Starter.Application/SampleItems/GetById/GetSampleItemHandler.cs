using Starter.Application.Common.Persistence;
using Starter.Application.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace Starter.Application.SampleItems.GetById;

public sealed class GetSampleItemHandler(StarterDbContext dbContext, HybridCache cache)
{
    public async Task<Result<GetSampleItemResult>> HandleAsync(GetSampleItemQuery query, CancellationToken cancellationToken)
    {
        var result = await cache.GetOrCreateAsync(
            $"sample-item:{query.Id.Value}",
            async cancellationToken =>
                await dbContext.SampleItems
                    .AsNoTracking()
                    .Where(x => x.Id == query.Id)
                    .Select(x => new GetSampleItemResult(
                        x.Id,
                        x.Name,
                        x.Code))
                    .SingleOrDefaultAsync(cancellationToken),
            cancellationToken: cancellationToken);

        if (result is null)
        {
            return Result<GetSampleItemResult>.Failure(SampleItemErrors.NotFound);
        }

        return Result<GetSampleItemResult>.Success(result);
    }
}
