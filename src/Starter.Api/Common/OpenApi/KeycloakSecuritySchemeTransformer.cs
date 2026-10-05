using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Starter.Api.Common.OpenApi;

public sealed class KeycloakSecuritySchemeTransformer(IConfiguration configuration) : IOpenApiDocumentTransformer
{
    public const string SchemeName = "OpenIdConnect";

    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authority = configuration["Keycloak:Authority"] ?? throw new InvalidOperationException("Keycloak authority is not configured.");

        var realm = configuration["Keycloak:Realm"] ?? throw new InvalidOperationException("Keycloak realm is not configured.");

        var discoveryUrl = $"{authority.TrimEnd('/')}/realms/{realm}/.well-known/openid-configuration";

        document.Components ??= new OpenApiComponents();

        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
        {
            [SchemeName] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OpenIdConnect,
                OpenIdConnectUrl = new Uri(discoveryUrl)
            }
        };

        return Task.CompletedTask;
    }
}
