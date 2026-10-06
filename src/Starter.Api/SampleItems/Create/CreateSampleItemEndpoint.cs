using Starter.Api.Common.Authorization;
using Starter.Api.Common.Http;
using Starter.Application.SampleItems.Create;
using Starter.Domain.SampleItems;

namespace Starter.Api.SampleItems.Create;

public static class CreateSampleItemEndpoint
{
    public static void MapCreateSampleItem(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/sample-items", async ( CreateSampleItemRequest request, CreateSampleItemHandler handler, CancellationToken cancellationToken) =>
        {
            var command = new CreateSampleItemCommand(request.Name, new SampleItemCode(request.Code));

            var result = await handler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.ToProblem();
            }

            var response = new CreateSampleItemResponse(result.Value.Id.Value);

            return Results.Created($"/api/sample-items/{response.Id}", response);
        })
        .WithName("CreateSampleItem")
        .WithTags("SampleItems")
        .RequireAuthorization(AuthorizationPolicies.CanManageSampleItems);
    }
}
