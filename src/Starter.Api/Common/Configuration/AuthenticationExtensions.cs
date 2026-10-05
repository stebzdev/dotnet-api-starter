using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Starter.Api.Common.Authentication;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddStarterAuthentication(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var realm = configuration["Keycloak:Realm"] ?? throw new InvalidOperationException("Keycloak realm is not configured.");

        var audience = configuration["Keycloak:Audience"] ?? throw new InvalidOperationException("Keycloak audience is not configured.");

        var issuer = configuration["Keycloak:Issuer"] ?? throw new InvalidOperationException("Keycloak issuer is not configured.");

        var keycloakUrl = configuration["services:keycloak:http:0"] ?? throw new InvalidOperationException("Keycloak URL is not configured.");

        var metadataAddress = $"{keycloakUrl.TrimEnd('/')}/realms/{realm}/.well-known/openid-configuration";


        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddKeycloakJwtBearer(
                serviceName: "keycloak",
                realm: realm,
                options =>
                {
                    options.Audience = audience;
                    options.TokenValidationParameters.ValidIssuer = issuer;

                    if (environment.IsDevelopment())
                    {
                        options.RequireHttpsMetadata = false;
                        options.Authority = null;
                        options.MetadataAddress = metadataAddress;
                        options.Backchannel = new HttpClient();
                    }
                });

        return services;
    }
}
