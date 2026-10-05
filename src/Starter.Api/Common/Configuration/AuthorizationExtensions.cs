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
                AuthorizationPolicies.CanManageReportingEntities,
                policy => policy.RequireRole(ApplicationRoles.Admin))
            .AddPolicy(
                AuthorizationPolicies.CanCreateReportingPeriod,
                policy => policy.RequireRole(ApplicationRoles.User, ApplicationRoles.Admin));
                    
        return services;
    }
}
