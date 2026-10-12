namespace Starter.Api.Tests.Setup;

public abstract class ApiIntegrationTest(ApiFixture fixture) : IAsyncLifetime
{
    protected HttpClient Client { get; } = fixture.Factory.CreateClient();

    public async Task InitializeAsync()
    {
        await fixture.ResetDatabaseAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
