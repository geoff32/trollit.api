using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrollIt.Application.Accounts.Abstractions;
using TrollIt.Application.Accounts.Models;

namespace TrollIt.Api.Account;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IAccountsService _accountService;

    public AccountController(IAccountsService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> CreateAccountAsync([FromBody] CreateAccountRequest createAccountRequest, CancellationToken cancellationToken)
    {
        var account = await  _accountService.CreateAccountAsync(createAccountRequest, cancellationToken);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, GetIdentity(account));
        return Ok(account);
    }

    [HttpPost("validate"), Authorize]
    public async Task<IActionResult> ValidateAsync(CancellationToken cancellationToken)
    {
        var account = await  _accountService.GetAccountAsync(Guid.Parse(User.Identity!.Name!), cancellationToken);

        return Ok(account);
    }

    [HttpPost("signin"), AllowAnonymous]
    public async Task<IActionResult> SignInAsync(AuthenticateRequest authenticateRequest, CancellationToken cancellationToken)
    {
        var account = await  _accountService.AuthenticateAsync(authenticateRequest, cancellationToken);

        if (account == null)
        {
            return Unauthorized(new { Message = "Identifiant ou mot de passe invalide" });
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
        identity.AddClaim(new Claim(ClaimTypes.Name, account.UserId.ToString()));

        return new ClaimsPrincipal(identity);
    }
}