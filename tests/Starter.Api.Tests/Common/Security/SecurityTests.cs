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
        var response = await Client.PostAsJsonAsync(
            "/api/sample-items",
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
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/sample-items")
            .AuthenticateAsTestUser(ApplicationRoles.Admin);

        request.Content = JsonContent.Create(new
        {
            Name = "Aurora Financial Group",
            Code = "AUTH-TEST"
        });

        using var response = await Client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task AdminEndpoint_WithoutRequiredRole_ShouldReturnForbidden()
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/sample-items")
            .AuthenticateAsTestUser();

        request.Content = JsonContent.Create(new
        {
            Name = "Aurora Financial Group",
            Code = "AUTH-TEST"
        });

        var response = await Client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task CreateSampleItem_WithAdminRole_ShouldBeAccessible()
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/sample-items")
            .AuthenticateAsTestUser(ApplicationRoles.Admin);

        request.Content = JsonContent.Create(new
        {
            Name = "Sample Item",
            Code = "AUTH-001"
        });

        using var response = await Client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateSampleItem_WithUserRole_ShouldReturnForbidden()
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/sample-items")
            .AuthenticateAsTestUser(ApplicationRoles.User);

        request.Content = JsonContent.Create(new
        {
            Name = "Sample Item",
            Code = "AUTH-002"
        });

        using var response = await Client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
