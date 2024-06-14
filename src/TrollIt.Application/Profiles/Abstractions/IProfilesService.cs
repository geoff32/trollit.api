using TrollIt.Application.Profiles.Models;

namespace TrollIt.Application.Profiles.Abstractions;

public interface IProfilesService
{
    Task<ProfileResponse?> RefreshProfileAsync(AppUser user, int trollId, CancellationToken cancellationToken);
    Task<ProfileResponse?> GetProfileAsync(AppUser user, int trollId, CancellationToken cancellationToken);
}
