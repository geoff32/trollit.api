using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Acl.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;
using TrollIt.Infrastructure.Accounts.Acl.Abstractions;
using TrollIt.Infrastructure.Accounts.Models;
using TrollIt.Infrastructure.Shares.Acl;
using TrollIt.Infrastructure.Shares.Models;

namespace TrollIt.Infrastructure.Accounts.Acl;

internal class AccountsRepositoryAcl(IAccountsAcl accountAcl) : IAccountsRepositoryAcl
{
    public IAccount? ToDomain(Account? account)
        => account == null ? null : accountAcl.ToDomain(
            new AccountDto(account.Id, account.Login, new TrollDto(account.TrollId, account.TrollName, account.ScriptToken)),
            account.Password);

    public IAccountPolicy ToDomain(int trollId, IEnumerable<Policy> policies)
    {
        return accountAcl.ToDomain(new AccountPolicyDto(trollId, MergeToRights(policies.SelectMany(policy => policy.Trolls))));
    }

    private static IEnumerable<TrollRightDto> MergeToRights(IEnumerable<TrollShare> trolls)
    {
        return trolls.Where(troll => troll.Status != PolicyStatus.Guest)
            .GroupBy(troll => troll.Trollid)
            .Select(memberRights => new TrollRightDto(memberRights.Key, Initialize(MergeToDto(memberRights.SelectMany(m => m.Features)))));
    }

    private static IEnumerable<FeatureDto> MergeToDto(IEnumerable<TrollFeature> features)
    {
        return features.GroupBy(f => ToAccountDomain(f.Id))
            .Select(group =>
                new FeatureDto
                (
                    group.Key,
                    group.Any(f => SharesRepositoryAcl.CanRead(f.Status)), group.Any(f => SharesRepositoryAcl.CanRefresh(f.Status))
                )
            );
    }

    private static Domain.Accounts.Abstractions.FeatureId ToAccountDomain(Shares.Models.FeatureId id) => id switch
    {
        Shares.Models.FeatureId.Profile => Domain.Accounts.Abstractions.FeatureId.Profile,
        Shares.Models.FeatureId.View => Domain.Accounts.Abstractions.FeatureId.View,
        _ => throw new ArgumentOutOfRangeException(nameof(id), id, null),
    };

    private static IEnumerable<FeatureDto> Initialize(IEnumerable<FeatureDto> features)
    {
        foreach (var featureId in Enum.GetValues<Domain.Accounts.Abstractions.FeatureId>())
        {
            var feature = features.FirstOrDefault(f => f.Id == featureId);

            yield return feature is not null ? feature : new FeatureDto(featureId, false, false);
        }
    }
}
