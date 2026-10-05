using Starter.Api.Common.Authorization;
using Starter.Api.Common.Http;
using Starter.Application.SampleItems.Create;
using Starter.Domain.SampleItems;

namespace Starter.Api.SampleItems.Create;

public static class CreateSampleItemEndpoint
{
    public static void MapCreateSampleEntity(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/sample-entities", async ( CreateSampleItemRequest request, CreateSampleItemHandler handler, CancellationToken cancellationToken) =>
        {
            var command = new CreateSampleItemCommand(request.Name, new SampleItemCode(request.Code));

            var result = await handler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.ToProblem();
            }

            var response = new CreateSampleItemResponse(result.Value.Id.Value);

            return Results.Created($"/api/sample-entities/{response.Id}", response);
        })
        .WithName("CreateSampleEntity")
        .WithTags("SampleEntities")
        .RequireAuthorization(AuthorizationPolicies.CanManageSampleEntities);
    }
}
