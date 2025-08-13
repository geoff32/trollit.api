using System.Diagnostics.CodeAnalysis;
using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;

namespace TrollIt.Domain.Accounts;

internal record AccountPolicy(int TrollId, IEnumerable<ITrollRight> Rights) : IAccountPolicy
{
    public AccountPolicy(AccountPolicyDto accountPolicyDto) : this(accountPolicyDto.Id, accountPolicyDto.Rights.Select(right => new TrollRight(right)))
    {
    }

    public bool CanRead(FeatureId featureId, int trollId) =>
        IsSameTroll(trollId) || TryGetFeature(trollId, featureId, out var feature) && feature.CanRead;
    
    public bool CanRefresh(FeatureId featureId, int trollId) => 
        IsSameTroll(trollId) || TryGetFeature(trollId, featureId, out var feature) && feature.CanRefresh;

    private bool IsSameTroll(int trollId) => TrollId == trollId;

    private bool TryGetFeature(int userTrollId, FeatureId featureId, [NotNullWhen(true)] out IFeature? feature)
    {
        var right = Rights.FirstOrDefault(right => right.TrollId == userTrollId);
        if (right == null)
        {
            feature = null;
            return false;
        }

        feature = right.Features.FirstOrDefault(f => f.Id == featureId);

        return feature != null;
    }
}
