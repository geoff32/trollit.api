using TrollIt.Domain.Profiles.Abstractions;

namespace TrollIt.Domain.Profiles.Infrastructure;

public interface IProfilesRepository
{
    Task<IProfile?> GetProfileAsync(int trollId, CancellationToken cancellationToken);
    Task<IProfile> RefreshProfileAsync(int trollId, string token, CancellationToken cancellationToken);
}