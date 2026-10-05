using Starter.Api.Common.OpenApi;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace Starter.Api.Tests.Common.OpenApi;

public sealed class KeycloakSecuritySchemeTransformerTests
{
    [Fact]
    public async Task TransformAsync_WhenConfigurationIsValid_ShouldAddOpenIdConnectSecurityScheme()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Keycloak:Authority"] = "https://localhost:8080/",
                ["Keycloak:Realm"] = "Starter"
            })
            .Build();

        var transformer = new KeycloakSecuritySchemeTransformer(configuration);
        var document = new OpenApiDocument();

        using var serviceProvider = new ServiceCollection().BuildServiceProvider();
        var context = CreateContext(serviceProvider);

        await transformer.TransformAsync(document, context, CancellationToken.None);

        Assert.NotNull(document.Components);
        Assert.NotNull(document.Components.SecuritySchemes);

        var scheme = Assert.IsType<OpenApiSecurityScheme>(document.Components.SecuritySchemes[KeycloakSecuritySchemeTransformer.SchemeName]);

        Assert.Equal(SecuritySchemeType.OpenIdConnect, scheme.Type);
        Assert.Equal(
            new Uri("https://localhost:8080/realms/Starter/.well-known/openid-configuration"),
            scheme.OpenIdConnectUrl);
    }

    [Fact]
    public async Task TransformAsync_WhenAuthorityIsMissing_ShouldThrowInvalidOperationException()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Keycloak:Realm"] = "Starter"
            })
            .Build();

        var transformer = new KeycloakSecuritySchemeTransformer(configuration);
        var document = new OpenApiDocument();

        using var serviceProvider = new ServiceCollection().BuildServiceProvider();
        var context = CreateContext(serviceProvider);

        Task Action() => transformer.TransformAsync(document, context, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(Action);

        Assert.Equal("Keycloak authority is not configured.", exception.Message);
    }

    [Fact]
    public async Task TransformAsync_WhenRealmIsMissing_ShouldThrowInvalidOperationException()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Keycloak:Authority"] = "https://localhost:8080/"
            })
            .Build();

        var transformer = new KeycloakSecuritySchemeTransformer(configuration);
        var document = new OpenApiDocument();

        using var serviceProvider = new ServiceCollection().BuildServiceProvider();
        var context = CreateContext(serviceProvider);

        Task Action() => transformer.TransformAsync(document, context, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(Action);

        Assert.Equal("Keycloak realm is not configured.", exception.Message);
    }

    private static OpenApiDocumentTransformerContext CreateContext(IServiceProvider serviceProvider)
    {
        return new OpenApiDocumentTransformerContext
        {
            DocumentName = "v1",
            DescriptionGroups = [],
            ApplicationServices = serviceProvider
        };
    }
}
