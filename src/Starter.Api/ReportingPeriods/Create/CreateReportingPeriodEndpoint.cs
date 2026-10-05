using Starter.Api.Common.Authorization;
using Starter.Api.Common.Http;
using Starter.Application.ReportingPeriods.Create;
using Starter.Domain.ReportingEntities;
using Starter.Domain.ReportingPeriods;

namespace Starter.Api.ReportingPeriods.Create;

public static class CreateReportingPeriodEndpoint
{
    public static void MapCreateReportingPeriod(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/reporting-periods", async (CreateReportingPeriodRequest request, CreateReportingPeriodHandler handler, CancellationToken cancellationToken) =>
        {
            var command = new CreateReportingPeriodCommand(
                new ReportingEntityId(request.ReportingEntityId),
                new ReferencePeriod(request.ReferenceYear, request.ReferenceMonth));

            var result = await handler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.ToProblem();
            }

            var response = new CreateReportingPeriodResponse(result.Value.Id.Value);

            return Results.Created($"api/reporting-periods/{response.Id}", response);
        })
        .WithName("CreateReportingPeriod")
        .WithTags("ReportingPeriods")
        .RequireAuthorization(AuthorizationPolicies.CanCreateReportingPeriod);
    }
}
