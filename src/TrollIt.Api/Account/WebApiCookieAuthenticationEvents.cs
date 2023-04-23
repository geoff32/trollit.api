using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TrollIt.Api.Account;

public class WebApiCookieAuthenticationEvents : CookieAuthenticationEvents
{

    public override async Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
    {
        await ReturnStatus(context.Response, StatusCodes.Status403Forbidden);
    }

    public async override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
    {
        await ReturnStatus(context.Response, StatusCodes.Status401Unauthorized);
    }

    public async override Task RedirectToLogout(RedirectContext<CookieAuthenticationOptions> context)
    {
        await ReturnStatus(context.Response, StatusCodes.Status401Unauthorized);
    }

    public async override Task RedirectToReturnUrl(RedirectContext<CookieAuthenticationOptions> context)
    {
        await ReturnStatus(context.Response, StatusCodes.Status200OK);
    }

    private async Task ReturnStatus(HttpResponse response, int status)
    {
        response.StatusCode = status;
        await Task.CompletedTask;
    }
}