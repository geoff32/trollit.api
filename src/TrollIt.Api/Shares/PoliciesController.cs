using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrollIt.Application.Shares.Abstractions;
using TrollIt.Application.Shares.Models;

namespace TrollIt.Api.Shares;

[ApiController]
[Route("api/policies")]
[Authorize]
public class PoliciesController(ISharesService sharesService) : ControllerBase
{
    [HttpGet("{policyId}", Name = "GetPolicy")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid policyId)
    {
        var policy =
            await sharesService.GetPolicyAsync(this.GetAppUserFromClaims(), policyId,
                HttpContext.RequestAborted);
        if (policy == null)
        {
            return NotFound();
        }

        return Ok(policy);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePolicyRequest request)
    {
        var policy =
            await sharesService.CreatePolicyAsync(this.GetAppUserFromClaims(), request,
                HttpContext.RequestAborted);

        return CreatedAtRoute("GetPolicy", new { policyId = policy.Id }, policy);
    }
    
    [HttpPost("{policyId:guid}/invitations", Name = "SendInvitation")]
    public async Task<IActionResult> SendInvitation([FromRoute]Guid policyId, [FromBody] SendInvitationRequest request) =>
        Ok(await sharesService.SendInvitationAsync(this.GetAppUserFromClaims(), policyId, request, HttpContext.RequestAborted));
}