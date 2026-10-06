using Starter.Api.Common.Authorization;

namespace Starter.Api.Common.Configuration;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddStarterAuthorization(
        this IServiceCollection services)
    {
        services
            .AddAuthorizationBuilder()
            .AddPolicy(
                AuthorizationPolicies.CanManageSampleItems,
                policy => policy.RequireRole(ApplicationRoles.Admin));
                    
        return services;
    }
}
