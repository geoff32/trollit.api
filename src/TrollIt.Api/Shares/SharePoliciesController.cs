using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrollIt.Application.Shares.Abstractions;
using TrollIt.Application.Shares.Models;

namespace TrollIt.Api;

[ApiController]
[Route("api/shares/policies")]
[Authorize]
public class SharePoliciesController(ISharesService sharesService) : ControllerBase
{
    [HttpGet("{sharePolicyId}", Name = "GetSharePolicy")]
    public async Task<IActionResult> GetSharePolicyAsync([FromRoute] Guid sharePolicyId)
    {
        var policy = await sharesService.GetSharePolicyAsync(this.GetAppUserFromClaims(), sharePolicyId, HttpContext.RequestAborted);
        if (policy == null)
        {
            return NotFound();
        }

        return Ok(policy);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSharePolicyAsync([FromBody] CreateSharePolicyRequest request)
    {
        var policy = await sharesService.CreateSharePolicyAsync(this.GetAppUserFromClaims(), request, HttpContext.RequestAborted);

        return CreatedAtRoute("GetSharePolicy", new { sharePolicyId = policy.Id}, policy);
    }
}
