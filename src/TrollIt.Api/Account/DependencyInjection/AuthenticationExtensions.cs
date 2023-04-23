using Microsoft.AspNetCore.Authentication.Cookies;

namespace TrollIt.Api.Account.DependencyInjection;

public static class AuthenticationExtensions
{
    public static void AddWebApiAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.EventsType = typeof(WebApiCookieAuthenticationEvents);
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
            });

        services.AddScoped<WebApiCookieAuthenticationEvents>();
    }
}