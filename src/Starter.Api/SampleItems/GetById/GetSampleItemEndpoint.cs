using Starter.Api.Common.Http;
using Starter.Application.SampleItems.GetById;
using Starter.Domain.SampleItems;

namespace Starter.Api.SampleItems.GetById;

public static class GetSampleItemEndpoint
{
    public static void MapGetSampleItem(this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/sample-items/{id:guid}",
            async (
                Guid id,
                GetSampleItemHandler handler,
                CancellationToken cancellationToken) =>
            {
                var query = new GetSampleItemQuery(new SampleItemId(id));

                var result = await handler.HandleAsync(query, cancellationToken);

                if (result.IsFailure)
                {
                    return result.Error.ToProblem();
                }

                var response = new GetSampleItemResponse(
                    result.Value.Id.Value,
                    result.Value.Name,
                    result.Value.Code.Value);

                return Results.Ok(response);
            })
            .WithName("GetSampleItem")
            .WithTags("SampleItems")
            .RequireAuthorization();
    }
}
