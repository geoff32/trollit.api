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
