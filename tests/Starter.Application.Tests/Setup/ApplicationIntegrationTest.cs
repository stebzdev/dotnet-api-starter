using Microsoft.EntityFrameworkCore;
using Starter.Application.Common.Persistence;

namespace Starter.Application.Tests.Setup;
public abstract class ApplicationIntegrationTest : IAsyncLifetime
{
    private readonly PostgreSqlFixture _fixture;

    protected  StarterDbContext DbContext { get; }

    protected ApplicationIntegrationTest(PostgreSqlFixture fixture)
    {
        _fixture = fixture;

        var options = new DbContextOptionsBuilder<StarterDbContext>()
            .UseNpgsql(fixture.ConnectionString)
            .Options;

        DbContext = new StarterDbContext(options);
    }

    public async Task InitializeAsync()
    {
        await _fixture.ResetDatabaseAsync();
    }

    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
    }
}
