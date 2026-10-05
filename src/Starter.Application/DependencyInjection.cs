using System.Text.Json;
using Starter.Application.Common.Caching;
using Starter.Application.Common.Persistence;
using Starter.Application.SampleItems.Create;
using Starter.Application.SampleItems.GetById;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Starter.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StarterDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("StarterDb");
            options.UseNpgsql(connectionString);
        });

        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.Converters.Add(new SampleItemCodeJsonConverter());
        services.AddKeyedSingleton<JsonSerializerOptions>(typeof(IHybridCacheSerializer<>), jsonSerializerOptions);

        services.AddHybridCache(options =>
        {
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(30),
                LocalCacheExpiration = TimeSpan.FromMinutes(5)
            };
        });

        services.AddScoped<CreateSampleItemHandler>();
        services.AddScoped<GetSampleItemHandler>();

        return services;
    }
}   
