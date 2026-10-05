using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Starter.Api.Tests.Setup;

public sealed class TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(TestAuthDefaults.UserHeader, out var _))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, TestAuthDefaults.UserName),
            new(ClaimTypes.Name, "Test User")
        };

        if (Request.Headers.TryGetValue(TestAuthDefaults.RolesHeader, out var roles))
        {
            claims.AddRange(roles.ToString()
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(role => new Claim(ClaimTypes.Role, role.Trim())));
        }

        var identity = new ClaimsIdentity(claims, TestAuthDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        var ticket = new AuthenticationTicket(principal, TestAuthDefaults.AuthenticationScheme);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

internal static class TestAuthDefaults
{
    internal const string AuthenticationScheme = "Test";
    internal const string UserHeader = "X-Test-User";
    internal const string RolesHeader = "X-Test-Roles";
    internal const string UserName = "test-user";
}
