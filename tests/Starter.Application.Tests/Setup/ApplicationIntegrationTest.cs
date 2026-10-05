using Starter.Application.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Starter.Application.Tests.Setup;
public abstract class ApplicationIntegrationTest : IAsyncLifetime
{
    private readonly PostgreSqlFixture _fixture;

    protected readonly StarterDbContext _dbContext;

    protected ApplicationIntegrationTest(PostgreSqlFixture fixture)
    {
        _fixture = fixture;

        var options = new DbContextOptionsBuilder<StarterDbContext>()
            .UseNpgsql(fixture.ConnectionString)
            .Options;

        _dbContext = new StarterDbContext(options);
    }

    public async Task InitializeAsync()
    {
        await _fixture.ResetDatabaseAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }
}
