using Microsoft.Extensions.Hosting;

namespace Starter.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddRedisDistributedCache("cache");

        return builder;
    }
}
