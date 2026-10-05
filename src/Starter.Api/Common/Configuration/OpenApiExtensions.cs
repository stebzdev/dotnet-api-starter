using Starter.Api.Common.OpenApi;
using Scalar.AspNetCore;

namespace Starter.Api.Common.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddStarterOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<KeycloakSecuritySchemeTransformer>();
        });

        return services;
    }

    public static WebApplication UseStarterOpenApi(this WebApplication app)
    {
        app.MapOpenApi();

        app.MapScalarApiReference(options =>
            options.AddPreferredSecuritySchemes(KeycloakSecuritySchemeTransformer.SchemeName));

        return app;
    }
}
