using Microsoft.AspNetCore.Mvc;
using TrollIt.Application.Profiles.Models;
using TrollIt.Application.Profiles.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace TrollIt.Api.Controllers
{
    [ApiController]
    [Route("api/profiles")]
    [Authorize]
    public class ProfilesController(IProfilesService profilesService) : ControllerBase
    {
        private readonly IProfilesService _profilesService = profilesService;

        [HttpGet("{trollId}")]
        public async Task<ActionResult<ProfileResponse>> GetProfileAsync(int trollId)
        {
            var profile = await _profilesService.GetProfileAsync(this.GetAppUserFromClaims(), trollId, HttpContext.RequestAborted);
            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        [HttpPost("{trollId}")]
        public async Task<ActionResult<ProfileResponse>> RefreshProfileAsync([FromRoute] int trollId)
        {
            var profile = await _profilesService.RefreshProfileAsync(this.GetAppUserFromClaims(), trollId, HttpContext.RequestAborted);
            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }
    }
}