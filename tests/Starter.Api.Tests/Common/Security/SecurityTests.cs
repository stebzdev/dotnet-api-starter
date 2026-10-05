using System.Net;
using System.Net.Http.Json;
using Starter.Api.Common.Authorization;
using Starter.Api.SampleItems.Create;
using Starter.Api.Tests.Setup;

namespace Starter.Api.Tests.Common.Security;

[Collection(ApiTestCollection.Name)]
public sealed class SecurityTests(ApiFixture fixture) : ApiIntegrationTest(fixture)
{
    [Fact]
    public async Task ProtectedEndpoint_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        var response = await _client.PostAsJsonAsync(
            "/api/sample-entities",
            new
            {
                Name = "Aurora Financial Group",
                Code = "AUTH-TEST"
            });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ProtectedEndpoint_WithRequiredRole_ShouldBeAccessible()
    {
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/sample-entities")
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
            "/api/sample-entities")
            .AuthenticateAsTestUser();

        request.Content = JsonContent.Create(new
        {
            Name = "Aurora Financial Group",
            Code = "AUTH-TEST"
        });

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

  /*  [Fact]
    public async Task CreateSampleItem_WithAdminRole_ShouldBeAccessible()
    {
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/sample-items")
            .AuthenticateAsTestUser(ApplicationRoles.Admin);

        request.Content = JsonContent.Create(new
        {
            Name = "Sample Item",
            Code = "AUTH-001"
        });

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateSampleItem_WithUserRole_ShouldReturnForbidden()
    {
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/sample-items")
            .AuthenticateAsTestUser(ApplicationRoles.User);

        request.Content = JsonContent.Create(new
        {
            Name = "Sample Item",
            Code = "AUTH-002"
        });

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }*/
}
