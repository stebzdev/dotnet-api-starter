using Starter.Application.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Starter.Api.Tests.Setup;

public sealed class ApiFixture : IAsyncLifetime
{
    private PostgreSqlFixture _postgres = null!;

    public StarterApiFactory Factory { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        _postgres = new PostgreSqlFixture();

        await _postgres.InitializeAsync();

        Factory = new StarterApiFactory(_postgres);

        using var scope = Factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<StarterDbContext>();

        await dbContext.Database.MigrateAsync();

        await _postgres.InitializeRespawnerAsync();
    }

    public async Task DisposeAsync()
    {
        await Factory.DisposeAsync();
        await _postgres.DisposeAsync();
    }

    public Task ResetDatabaseAsync()
    {
        return _postgres.ResetDatabaseAsync();
    }
}
