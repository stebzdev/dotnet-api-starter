using System.Net;
using System.Net.Http.Json;
using Starter.Api.Common.Authorization;
using Starter.Api.SampleItems.Create;
using Starter.Api.Tests.Setup;

namespace Starter.Api.Tests.SampleItems.Create;

[Collection(ApiTestCollection.Name)]
public sealed class CreateSampleItemEndpointTests(ApiFixture fixture) : ApiIntegrationTest(fixture)
{
    [Fact]
    public async Task Create_WithValidRequest_ShouldReturnCreated()
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/sample-items")
            .AuthenticateAsTestUser(ApplicationRoles.Admin);

        request.Content = JsonContent.Create(new
        {
            Name = "Aurora Financial Group",
            Code = "AFG"
        });

        using var response = await Client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<CreateSampleItemResponse>();

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public async Task Create_WithDuplicateCode_ShouldReturnConflict()
    {
        using var firstRequest = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/sample-items")
            .AuthenticateAsTestUser(ApplicationRoles.Admin);

        firstRequest.Content = JsonContent.Create(new
        {
            Name = "Aurora Financial Group",
            Code = "AFG"
        });

        using var firstResponse = await Client.SendAsync(firstRequest);

        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);

        using var secondRequest = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/sample-items")
            .AuthenticateAsTestUser(ApplicationRoles.Admin);

        secondRequest.Content = JsonContent.Create(new
        {
            Name = "Another Financial Group",
            Code = "AFG"
        });

        using var response = await Client.SendAsync(secondRequest);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithInvalidCode_ShouldReturnBadRequest()
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/sample-items")
            .AuthenticateAsTestUser(ApplicationRoles.Admin);

        request.Content = JsonContent.Create(new
        {
            Name = "Aurora Financial Group",
            Code = " "
        });

        using var response = await Client.SendAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

}
