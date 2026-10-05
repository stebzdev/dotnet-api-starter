using System.Net;
using System.Net.Http.Json;
using Starter.Api.Common.Authorization;
using Starter.Api.ReportingEntities.Create;
using Starter.Api.ReportingEntities.GetById;
using Starter.Api.Tests.Setup;

namespace Starter.Api.Tests.ReportingEntities.GetById;

[Collection(ApiTestCollection.Name)]
public sealed class GetReportingEntityEndpointTests(ApiFixture fixture) : ApiIntegrationTest(fixture)
{
    private readonly ApiFixture _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));

    [Theory]
    [InlineData("AFG", "Aurora Financial Group")]
    public async Task Get_WithExistingEntity_ShouldReturnOk(string code, string name)
    {
        var createRequest = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/reporting-entities")
            .AuthenticateAsTestUser(ApplicationRoles.Admin);

        createRequest.Content = JsonContent.Create(new
        {
            Name = name,
            Code = code
        });

        var createResponse = await _client.SendAsync(createRequest);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdEntity = await createResponse.Content.ReadFromJsonAsync<CreateReportingEntityResponse>();

        Assert.NotNull(createdEntity);

        var getRequest = new HttpRequestMessage(
            HttpMethod.Get,
            $"/api/reporting-entities/{createdEntity.Id}")
            .AuthenticateAsTestUser();

        var response = await _client.SendAsync(getRequest);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<GetReportingEntityResponse>();

        Assert.NotNull(result);
        Assert.Equal(createdEntity.Id, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(code, result.Code);
    }

    [Fact]
    public async Task Get_WithNonExistingEntity_ShouldReturnNotFound()
    {
        var id = Guid.NewGuid();

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"/api/reporting-entities/{id}")
            .AuthenticateAsTestUser();

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        var id = Guid.NewGuid();

        var response = await _client.GetAsync($"/api/reporting-entities/{id}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
