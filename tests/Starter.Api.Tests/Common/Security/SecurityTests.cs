using System.Net;
using System.Net.Http.Json;
using Starter.Api.Common.Authorization;
using Starter.Api.ReportingEntities.Create;
using Starter.Api.Tests.Setup;

namespace Starter.Api.Tests.Common.Security;

[Collection(ApiTestCollection.Name)]
public sealed class SecurityTests(ApiFixture fixture) : ApiIntegrationTest(fixture)
{
    [Fact]
    public async Task ProtectedEndpoint_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        var response = await _client.PostAsJsonAsync(
            "/api/reporting-periods",
            new
            {
                ReportingEntityId = Guid.NewGuid(),
                ReferenceYear = 2026,
                ReferenceMonth = 9
            });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ProtectedEndpoint_WithRequiredRole_ShouldBeAccessible()
    {
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/reporting-entities")
            .AuthenticateAsTestUser(ApplicationRoles.Admin);

        request.Content = JsonContent.Create(new
        {
            Name = "Aurora Financial Group",
            Code = "AUTH-TEST"
        });

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task AdminEndpoint_WithoutRequiredRole_ShouldReturnForbidden()
    {
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/reporting-entities")
            .AuthenticateAsTestUser();

        request.Content = JsonContent.Create(new
        {
            Name = "Aurora Financial Group",
            Code = "AUTH-TEST"
        });

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task CreateReportingPeriod_WithUserRole_ShouldBeAccessible()
    {
        var createEntityRequest = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/reporting-entities")
            .AuthenticateAsTestUser(ApplicationRoles.Admin);

        createEntityRequest.Content = JsonContent.Create(new
        {
            Name = "Aurora Financial Group",
            Code = "AUTH-PERIOD"
        });

        var createEntityResponse = await _client.SendAsync(createEntityRequest);

        Assert.Equal(HttpStatusCode.Created, createEntityResponse.StatusCode);

        var entity = await createEntityResponse.Content.ReadFromJsonAsync<CreateReportingEntityResponse>();

        Assert.NotNull(entity);

        var createPeriodRequest = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/reporting-periods")
            .AuthenticateAsTestUser(ApplicationRoles.User);

        createPeriodRequest.Content = JsonContent.Create(new
        {
            ReportingEntityId = entity.Id,
            ReferenceYear = 2026,
            ReferenceMonth = 9
        });

        var response = await _client.SendAsync(createPeriodRequest);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateReportingPeriod_WithReviewerRole_ShouldReturnForbidden()
    {
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/reporting-periods")
            .AuthenticateAsTestUser(ApplicationRoles.Reviewer);

        request.Content = JsonContent.Create(new
        {
            ReportingEntityId = Guid.NewGuid(),
            ReferenceYear = 2026,
            ReferenceMonth = 9
        });

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
