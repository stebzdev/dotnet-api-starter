using System.Net;
using System.Net.Http.Json;
using Starter.Api.Common.Authorization;
using Starter.Api.ReportingEntities.Create;
using Starter.Api.ReportingPeriods.Create;
using Starter.Api.Tests.Setup;

namespace Starter.Api.Tests.ReportingPeriods.Create;

[Collection(ApiTestCollection.Name)]
public sealed class CreateReportingPeriodEndpointTests(ApiFixture fixture) : ApiIntegrationTest(fixture)
{
    [Fact]
    public async Task Create_WithValidRequest_ShouldReturnCreated()
    {
        var reportingEntityId = await CreateReportingEntityAsync();

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/reporting-periods")
            .AuthenticateAsTestUser(ApplicationRoles.User);

        request.Content = JsonContent.Create(new
        {
            ReportingEntityId = reportingEntityId,
            ReferenceYear = 2026,
            ReferenceMonth = 9
        });

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<CreateReportingPeriodResponse>();

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public async Task Create_WithNonExistingReportingEntity_ShouldReturnNotFound()
    {
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/reporting-periods")
            .AuthenticateAsTestUser(ApplicationRoles.User);

        request.Content = JsonContent.Create(new
        {
            ReportingEntityId = Guid.NewGuid(),
            ReferenceYear = 2026,
            ReferenceMonth = 9
        });

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithDuplicatePeriod_ShouldReturnConflict()
    {
        var reportingEntityId = await CreateReportingEntityAsync();

        var firstRequest = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/reporting-periods")
            .AuthenticateAsTestUser(ApplicationRoles.User);

        firstRequest.Content = JsonContent.Create(new
        {
            ReportingEntityId = reportingEntityId,
            ReferenceYear = 2026,
            ReferenceMonth = 9
        });

        var firstResponse = await _client.SendAsync(firstRequest);

        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);

        var secondRequest = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/reporting-periods")
            .AuthenticateAsTestUser(ApplicationRoles.User);

        secondRequest.Content = JsonContent.Create(new
        {
            ReportingEntityId = reportingEntityId,
            ReferenceYear = 2026,
            ReferenceMonth = 9
        });

        var response = await _client.SendAsync(secondRequest);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    private async Task<Guid> CreateReportingEntityAsync()
    {
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/reporting-entities")
            .AuthenticateAsTestUser(ApplicationRoles.Admin);

        request.Content = JsonContent.Create(new
        {
            Name = "Aurora Financial Group",
            Code = "AFG"
        });

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var entity = await response.Content.ReadFromJsonAsync<CreateReportingEntityResponse>();

        Assert.NotNull(entity);

        return entity.Id;
    }

    [Fact]
    public async Task Create_WithInvalidReferenceMonth_ShouldReturnBadRequest()
    {
        var reportingEntityId = await CreateReportingEntityAsync();

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/reporting-periods")
            .AuthenticateAsTestUser(ApplicationRoles.User);

        request.Content = JsonContent.Create(new
        {
            ReportingEntityId = reportingEntityId,
            ReferenceYear = 2026,
            ReferenceMonth = 13
        });

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
