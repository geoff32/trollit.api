using System.Diagnostics.CodeAnalysis;
using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;
using TrollIt.Domain.Shares.Exceptions;

namespace TrollIt.Domain.Shares;

internal record UserPolicy(int TrollId, IEnumerable<ITrollRight> Rights) : IUserPolicy
{
    public UserPolicy(UserPolicyDto userPolicyDto) : this(userPolicyDto.Id, userPolicyDto.Rights.Select(right => new TrollRight(right)))
    {
    }

    public void EnsureReadAccess(FeatureId featureId, int trollId)
    {
        if (TrollId != trollId && !CanRead(featureId, trollId))
        {
            throw new DomainException<SharesExceptions>(SharesExceptions.NoReadAccess);
        }
    }

    public void EnsureRefreshAccess(FeatureId featureId, int trollId)
    {
        if (TrollId != trollId && !CanRefresh(featureId, trollId))
        {
            throw new DomainException<SharesExceptions>(SharesExceptions.NoRefreshAccess);
        }
    }

    private bool CanRead(FeatureId featureId, int trollId)
    {
        return TryGetFeature(trollId, featureId, out var feature) && (feature.CanRead || feature.CanRefresh);
    }

    private bool CanRefresh(FeatureId featureId, int trollId)
    {
        return TryGetFeature(trollId, featureId, out var feature) && feature.CanRefresh;
    }

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
