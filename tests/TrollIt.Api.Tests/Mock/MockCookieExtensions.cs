using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace TrollIt.Api.Tests.Mock;

public static class MockCookieExtensions
{
    public static void AddMockCookieAuthentication(this IServiceCollection services, MockAuthenticatedUsersRepository mockAuthenticatedUsersRepository)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton(mockAuthenticatedUsersRepository);
        services.AddTransient<MockAuthenticationHandler>();
        
        services.AddAuthentication(MockAuthenticationHandler.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, MockAuthenticationHandler>(MockAuthenticationHandler.SchemeName, options => { });
    }
}
