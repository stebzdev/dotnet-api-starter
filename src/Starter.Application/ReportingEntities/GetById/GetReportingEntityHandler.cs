using Starter.Application.Common.Persistence;
using Starter.Application.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace Starter.Application.ReportingEntities.GetById;

public sealed class GetReportingEntityHandler(StarterDbContext dbContext, HybridCache cache)
{
    public async Task<Result<GetReportingEntityResult>> HandleAsync(GetReportingEntityQuery query, CancellationToken cancellationToken)
    {
        var result = await cache.GetOrCreateAsync(
            $"reporting-entity:{query.Id.Value}",
            async cancellationToken =>
                await dbContext.ReportingEntities
                    .AsNoTracking()
                    .Where(x => x.Id == query.Id)
                    .Select(x => new GetReportingEntityResult(
                        x.Id,
                        x.Name,
                        x.Code))
                    .SingleOrDefaultAsync(cancellationToken),
            cancellationToken: cancellationToken);

        if (result is null)
        {
            return Result<GetReportingEntityResult>.Failure(ReportingEntityErrors.NotFound);
        }

        return Result<GetReportingEntityResult>.Success(result);
    }
}
