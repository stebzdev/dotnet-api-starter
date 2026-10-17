using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Starter.Application.Common.Persistence;
using Starter.Application.Common.Results;

namespace Starter.Application.SampleItems.GetById;

public sealed class GetSampleItemHandler(StarterDbContext dbContext, HybridCache cache)
{
    public async Task<Result<GetSampleItemResult>> HandleAsync(GetSampleItemQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

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
            return SampleItemErrors.NotFound;
        }

        return Result<GetSampleItemResult>.Success(result);
    }
}
