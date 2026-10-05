using Starter.Api.Common.Authorization;
using Starter.Api.Common.Http;
using Starter.Application.ReportingEntities.Create;
using Starter.Domain.ReportingEntities;

namespace Starter.Api.ReportingEntities.Create;

public static class CreateReportingEntityEndpoint
{
    public static void MapCreateReportingEntity(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/reporting-entities", async ( CreateReportingEntityRequest request, CreateReportingEntityHandler handler, CancellationToken cancellationToken) =>
        {
            var command = new CreateReportingEntityCommand(request.Name, new ReportingEntityCode(request.Code));

            var result = await handler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.ToProblem();
            }

            var response = new CreateReportingEntityResponse(result.Value.Id.Value);

            return Results.Created($"/api/reporting-entities/{response.Id}", response);
        })
        .WithName("CreateReportingEntity")
        .WithTags("ReportingEntities")
        .RequireAuthorization(AuthorizationPolicies.CanManageReportingEntities);
    }
}
