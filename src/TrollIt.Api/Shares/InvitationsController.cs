using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrollIt.Application.Shares.Abstractions;
using TrollIt.Application.Shares.Models;

namespace TrollIt.Api.Shares;

[ApiController]
[Route("api/invitations")]
[Authorize]
public class InvitationsController(ISharesService sharesService) : ControllerBase
{
    [HttpGet(Name = "GetUserInvitations")]
    public IAsyncEnumerable<InvitationResponse> GetUserInvitationsAsync() =>
        sharesService.GetUserInvitationsAsync(this.GetAppUserFromClaims(), HttpContext.RequestAborted);

    [HttpPost("{invitationId:guid}", Name = "SendAnswer")]
    public async Task<IActionResult> SendAnswerAsync([FromRoute]Guid invitationId, [FromBody] SendAnswerRequest request) =>
        Ok(await sharesService.SendAnswerAsync(this.GetAppUserFromClaims(), invitationId, request, HttpContext.RequestAborted));
}
