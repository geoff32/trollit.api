using TrollIt.Application.Profiles.Abstractions;
using TrollIt.Application.Profiles.Models;
using TrollIt.Domain.Accounts.Infrastructure;
using TrollIt.Domain.Profiles.Infrastructure;
using TrollIt.Domain.Shares.Infrastructure;

namespace TrollIt.Application.Profiles;

internal class ProfilesService(IProfilesRepository profilesRepository, ISharesRepository sharesRepository, IAccountsRepository accountsRepository) : IProfilesService
{
    public async Task<ProfileResponse?> GetProfileAsync(AppUser user, int trollId, CancellationToken cancellationToken)
    {
        var userPolicy = await sharesRepository.GetUserPolicyAsync(user.TrollId, cancellationToken);
        userPolicy.EnsureReadAccess(Domain.Shares.Abstractions.FeatureId.Profile, trollId);

        var profile = await profilesRepository.GetProfileAsync(trollId, cancellationToken);

        return profile == null ? null : new ProfileResponse(profile);
    }

    public async Task<ProfileResponse?> RefreshProfileAsync(AppUser user, int trollId, CancellationToken cancellationToken)
    {
        var userPolicy = await sharesRepository.GetUserPolicyAsync(user.TrollId, cancellationToken);
        userPolicy.EnsureReadAccess(Domain.Shares.Abstractions.FeatureId.Profile, trollId);

        var account = await accountsRepository.GetAccountByTrollAsync(trollId, cancellationToken)
            ?? throw new AppException<ProfileExceptions>(ProfileExceptions.TrollNotFound);

        var profile = await profilesRepository.RefreshProfileAsync(trollId, account.Troll.ScriptToken, cancellationToken);

        return profile == null ? null : new ProfileResponse(profile);
    }
}