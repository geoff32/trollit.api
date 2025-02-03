using TrollIt.Application.Profiles.Models;

namespace TrollIt.Application.Profiles.Abstractions;

public interface IProfilesService
{
    Task<ProfileResponse?> RefreshProfileAsync(int trollId, CancellationToken cancellationToken);
    Task<ProfileResponse?> GetProfileAsync(int trollId, CancellationToken cancellationToken);
}
