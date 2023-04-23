using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrollIt.Application.Account.Abstractions;
using TrollIt.Application.Account.Models;

namespace TrollIt.Api.Account;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> CreateAccountAsync([FromBody] CreateAccountRequest createAccountRequest)
    {
        var account = await  _accountService.CreateAccountAsync(createAccountRequest);

        return Ok(account);
    }

    [HttpPost("validate"), Authorize]
    public async Task<IActionResult> ValidateAsync()
    {
        var account = await  _accountService.GetAccountAsync(Guid.Parse(User.Identity!.Name!));

        return Ok(account);
    }

    [HttpPost("signin"), AllowAnonymous]
    public async Task<IActionResult> SignInAsync(AuthenticateRequest authenticateRequest)
    {
        var account = await  _accountService.AuthenticateAsync(authenticateRequest);

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

    private ClaimsPrincipal GetIdentity(AccountResponse account)
    {
        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
        identity.AddClaim(new Claim(ClaimTypes.Name, account.UserId.ToString()));

        return new ClaimsPrincipal(identity);
    }
}