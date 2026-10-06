using System.Net;
using System.Net.Http.Json;
using Starter.Api.Common.Authorization;
using Starter.Api.SampleItems.Create;
using Starter.Api.SampleItems.GetById;
using Starter.Api.Tests.Setup;

namespace Starter.Api.Tests.SampleItems.GetById;

[Collection(ApiTestCollection.Name)]
public sealed class GetSampleItemEndpointTests(ApiFixture fixture) : ApiIntegrationTest(fixture)
{
    private readonly ApiFixture _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));

    [Theory]
    [InlineData("AFG", "Aurora Financial Group")]
    public async Task Get_WithExistingItem_ShouldReturnOk(string code, string name)
    {
        var createRequest = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/sample-items")
            .AuthenticateAsTestUser(ApplicationRoles.Admin);

        createRequest.Content = JsonContent.Create(new
        {
            Name = name,
            Code = code
        });

        var createResponse = await _client.SendAsync(createRequest);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdItem = await createResponse.Content.ReadFromJsonAsync<CreateSampleItemResponse>();

        Assert.NotNull(createdItem);

        var getRequest = new HttpRequestMessage(
            HttpMethod.Get,
            $"/api/sample-items/{createdItem.Id}")
            .AuthenticateAsTestUser();

        var response = await _client.SendAsync(getRequest);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<GetSampleItemResponse>();

        Assert.NotNull(result);
        Assert.Equal(createdItem.Id, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(code, result.Code);
    }

    [Fact]
    public async Task Get_WithNonExistingEntity_ShouldReturnNotFound()
    {
        var id = Guid.NewGuid();

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"/api/sample-items/{id}")
            .AuthenticateAsTestUser();

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        var id = Guid.NewGuid();

        var response = await _client.GetAsync($"/api/sample-items/{id}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
