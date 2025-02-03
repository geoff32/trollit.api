using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TrollIt.Application;

namespace TrollIt.Api;

public static class ControllerExtensions
{
    public static AppUser GetAppUserFromClaims(this ControllerBase controller) => GetAppUserFromClaims(controller.User);

    public static AppUser GetAppUserFromClaims(this ClaimsPrincipal claimsPrincipal)
    {
        var trollId = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value
            ?? throw new InvalidOperationException("TrollId claim not found.");
        var trollName = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value
            ?? throw new InvalidOperationException("TrollName claim not found.");
        var accountId = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.UserData)?.Value
            ?? throw new InvalidOperationException("UserId claim not found.");
            
        return new AppUser(Guid.Parse(accountId), int.Parse(trollId), trollName);
    }
}
