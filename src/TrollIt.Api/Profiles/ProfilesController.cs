using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrollIt.Api.Authorization;
using TrollIt.Application.Profiles.Abstractions;
using TrollIt.Application.Profiles.Models;
using TrollIt.Domain.Accounts.Abstractions;

namespace TrollIt.Api.Profiles
{
    [ApiController]
    [Route("api/profiles")]
    [Authorize]
    public class ProfilesController(IAuthorizationService authorizationService, IProfilesService profilesService)
        : ControllerBase
    {
        [HttpGet("{trollId}")]
        public async Task<ActionResult<ProfileResponse>> GetProfileAsync(int trollId)
        {
            var authorizationResult = await authorizationService.AuthorizeAsync(User,
                new TrollResource(trollId, FeatureId.Profile), TrollOperations.Read);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var profile = await profilesService.GetProfileAsync(trollId, HttpContext.RequestAborted);
            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        [HttpPost("{trollId}")]
        public async Task<ActionResult<ProfileResponse>> RefreshProfileAsync([FromRoute] int trollId)
        {
            var authorizationResult = await authorizationService.AuthorizeAsync(User,
                new TrollResource(trollId, FeatureId.Profile), TrollOperations.Refresh);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var profile = await profilesService.RefreshProfileAsync(trollId, HttpContext.RequestAborted);
            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }
    }
}