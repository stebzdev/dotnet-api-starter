using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Starter.Api.Common.Authorization;
using Starter.Api.Common.Exceptions;
using Starter.Api.Common.Http;
using Starter.Api.Tests.Setup;
using Starter.Domain.SampleItems;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace Starter.Api.Tests.Common.Exceptions;

[Collection(ApiTestCollection.Name)]
public sealed class GlobalExceptionHandlerTests(ApiFixture fixture) : ApiIntegrationTest(fixture)
{
    [Fact]
    public async Task DomainException_ShouldReturnBadRequest()
    {
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/sample-entities")
            .AuthenticateAsTestUser(ApplicationRoles.Admin);

        request.Content = JsonContent.Create(new
        {
            Name = "Aurora Financial Group",
            Code = ""
        });

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problemDetails);
        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.Equal(ProblemDetailsTitles.ValidationError, problemDetails.Title);
        Assert.Equal(SampleItemCode.InvalidCodeMessage, problemDetails.Detail);
        Assert.True(problemDetails.Extensions.ContainsKey("traceId"));
    }

    [Fact]
    public async Task UnexpectedException_ShouldReturnInternalServerError()
    {
        var handler = new GlobalExceptionHandler(NullLogger<GlobalExceptionHandler>.Instance);

        var httpContext = new DefaultHttpContext();

        await using var responseBody = new MemoryStream();

        httpContext.Response.Body = responseBody;
        httpContext.TraceIdentifier = "test-trace-id";

        var exception = new InvalidOperationException("Sensitive database information");

        var handled = await handler.TryHandleAsync(
            httpContext,
            exception,
            CancellationToken.None);

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status500InternalServerError, httpContext.Response.StatusCode);

        responseBody.Position = 0;
        using var response = await JsonDocument.ParseAsync(responseBody);
        var root = response.RootElement;

        Assert.Equal(ProblemDetailsTitles.UnexpectedError, root.GetProperty("title").GetString());
        Assert.Equal(StatusCodes.Status500InternalServerError, root.GetProperty("status").GetInt32());
        Assert.Equal("test-trace-id", root.GetProperty("traceId").GetString());
        Assert.DoesNotContain("Sensitive database information", root.GetRawText());
        Assert.DoesNotContain("InvalidOperationException", root.GetRawText());
    }
}
