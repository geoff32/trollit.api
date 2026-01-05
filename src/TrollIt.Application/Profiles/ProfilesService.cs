using TrollIt.Application.Profiles.Abstractions;
using TrollIt.Application.Profiles.Exceptions;
using TrollIt.Application.Profiles.Models;
using TrollIt.Domain.Accounts.Infrastructure;
using TrollIt.Domain.Profiles.Infrastructure;

namespace TrollIt.Application.Profiles;

internal class ProfilesService(IProfilesRepository profilesRepository, IAccountsRepository accountsRepository) : IProfilesService
{
    public async Task<ProfileResponse?> GetProfileAsync(int trollId, CancellationToken cancellationToken)
    {
        var profile = await profilesRepository.GetProfileAsync(trollId, cancellationToken);

        return profile == null ? null : new ProfileResponse(profile);
    }

    public async Task<ProfileResponse?> RefreshProfileAsync(int trollId, CancellationToken cancellationToken)
    {
        var account = await accountsRepository.GetAccountByTrollAsync(trollId, cancellationToken)
            ?? throw new AppException<ProfileExceptions>(ProfileExceptions.TrollNotFound);

        var profile = await profilesRepository.RefreshProfileAsync(trollId, account.Troll.ScriptToken, cancellationToken);

        return new ProfileResponse(profile);
    }
}