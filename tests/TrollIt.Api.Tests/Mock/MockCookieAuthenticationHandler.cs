using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TrollIt.Api.Tests.Mock;

public class MockAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IHttpContextAccessor contextAccessor,
    MockAuthenticatedUsersRepository mockAuthenticatedUsersRepository
    ) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "MockAuthentication";
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (contextAccessor.HttpContext?.Request.Headers.TryGetValue("Mock-Authenticated-UserId", out var users) ?? false)
        {
            var user = users.FirstOrDefault();
            if (user != null && Guid.TryParse(user, out var userId) && mockAuthenticatedUsersRepository.IsUserAuthenticated(userId))
            {
                var claims = new[] { new Claim(ClaimTypes.Name, userId.ToString()) };
                var identity = new ClaimsIdentity(claims, SchemeName);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, SchemeName);

                var result = AuthenticateResult.Success(ticket);

                return Task.FromResult(result);
            }
        }

        return Task.FromResult(AuthenticateResult.Fail("No user authenticated"));
    }
}
