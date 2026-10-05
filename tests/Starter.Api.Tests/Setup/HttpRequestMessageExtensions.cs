namespace Starter.Api.Tests.Setup;

internal static class HttpRequestMessageExtensions
{
    internal static HttpRequestMessage AuthenticateAsTestUser(this HttpRequestMessage request, params string[] roles)
    {
        request.Headers.Add(TestAuthDefaults.UserHeader, TestAuthDefaults.UserName);

        if (roles.Length > 0)
        {
            request.Headers.Add(TestAuthDefaults.RolesHeader,  string.Join(',', roles));
        }

        return request;
    }
}
