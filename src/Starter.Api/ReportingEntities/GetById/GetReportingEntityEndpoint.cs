using Starter.Api.Common.Http;
using Starter.Application.ReportingEntities.GetById;
using Starter.Domain.ReportingEntities;

namespace Starter.Api.ReportingEntities.GetById;

public static class GetReportingEntityEndpoint
{
    public static void MapGetReportingEntity(this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/reporting-entities/{id:guid}",
            async (
                Guid id,
                GetReportingEntityHandler handler,
                CancellationToken cancellationToken) =>
            {
                var query = new GetReportingEntityQuery(new ReportingEntityId(id));

                var result = await handler.HandleAsync(query, cancellationToken);

                if (result.IsFailure)
                {
                    return result.Error.ToProblem();
                }

                var response = new GetReportingEntityResponse(
                    result.Value.Id.Value,
                    result.Value.Name,
                    result.Value.Code.Value);

                return Results.Ok(response);
            })
            .WithName("GetReportingEntity")
            .WithTags("ReportingEntities")
            .RequireAuthorization();
    }
}
