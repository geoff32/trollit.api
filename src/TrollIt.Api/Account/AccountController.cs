using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using TrollIt.Application.Accounts.Abstractions;
using TrollIt.Application.Accounts.Models;

namespace TrollIt.Api.Account;

[ApiController]
[Route("api/account")]
public class AccountController(IAccountsService accountService, IStringLocalizer<AccountController> stringLocalizer) : ControllerBase
{
    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> CreateAccountAsync([FromBody] CreateAccountRequest createAccountRequest)
    {
        var account = await accountService.CreateAccountAsync(createAccountRequest, HttpContext.RequestAborted);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, GetIdentity(account));
        return Ok(account);
    }

    [HttpPost("validate"), Authorize]
    public async Task<IActionResult> ValidateAsync()
    {
        var account = await accountService.GetAccountAsync(this.GetAppUserFromClaims().AccountId, HttpContext.RequestAborted);

        return Ok(account);
    }

    [HttpPost("signin"), AllowAnonymous]
    public async Task<IActionResult> SignInAsync(AuthenticateRequest authenticateRequest)
    {
        var account = await accountService.AuthenticateAsync(authenticateRequest, HttpContext.RequestAborted);

        if (account == null)
        {
            return Unauthorized(ProblemDetailsFactory.CreateProblemDetails
            (
                HttpContext,
                statusCode: StatusCodes.Status401Unauthorized,
                title: stringLocalizer["Title error"],
                detail: stringLocalizer["Invalid login or password"]
            ));
        }

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, GetIdentity(account));
        return Ok(account);
    }

    [HttpPost("signout"), Authorize]
    public async Task<IActionResult> SignOutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }

    private static ClaimsPrincipal GetIdentity(AccountResponse account)
    {
        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
        identity.AddClaim(new Claim(ClaimTypes.Name, account.Troll.Name));
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, account.Troll.Id.ToString()));
        identity.AddClaim(new Claim(ClaimTypes.UserData, account.UserId.ToString()));

        return new ClaimsPrincipal(identity);
    }
}