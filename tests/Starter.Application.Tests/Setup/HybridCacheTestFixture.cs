using System.Text.Json;
using Starter.Application.Common.Caching;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;

namespace Starter.Application.Tests.Setup;

public sealed class HybridCacheTestFixture : IAsyncDisposable
{
    private readonly ServiceProvider _serviceProvider;
    public HybridCache Cache { get; }

    public HybridCacheTestFixture()
    {
        var services = new ServiceCollection();

        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.Converters.Add(new ReportingEntityCodeJsonConverter());
        services.AddKeyedSingleton<JsonSerializerOptions>(typeof(IHybridCacheSerializer<>), jsonSerializerOptions);

        services.AddHybridCache();

        _serviceProvider = services.BuildServiceProvider();

        Cache = _serviceProvider.GetRequiredService<HybridCache>();
    }

    public ValueTask DisposeAsync()
    {
        return _serviceProvider.DisposeAsync();
    }
}
