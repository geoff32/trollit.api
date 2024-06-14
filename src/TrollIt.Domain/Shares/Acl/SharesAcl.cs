using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;

namespace TrollIt.Domain.Shares.Acl;

public class SharesAcl : ISharesAcl
{
    public ISharePolicy ToDomain(SharePolicyDto policyDto) => new SharePolicy(Initialize(policyDto));

    public IUserPolicy ToDomain(int trollId, IEnumerable<ISharePolicy> sharePolicies)
    {
        return new UserPolicy(new UserPolicyDto(trollId, MergeToRights(sharePolicies.SelectMany(sharePolicy => sharePolicy.Members))));
    }

    private static IEnumerable<TrollRightDto> MergeToRights(IEnumerable<IMember> members)
    {
        return members.Where(member => !member.IsGuest)
            .GroupBy(member => member.Id)
            .Select(memberRights => new TrollRightDto(memberRights.Key, Initialize(MergeToDto(memberRights.SelectMany(m => m.Features)))));
    }

    private static IEnumerable<FeatureDto> MergeToDto(IEnumerable<IFeature> features)
    {
        return features.GroupBy(f => f.Id)
            .Select(group => new FeatureDto(group.Key, group.Any(f => f.CanRead), group.Any(f => f.CanRefresh)));
    }

    private static SharePolicyDto Initialize(SharePolicyDto sharePolicyDto)
    {
        return new SharePolicyDto(sharePolicyDto.Id, sharePolicyDto.Name, sharePolicyDto.Members.Select(Initialize));
    }

    private static MemberDto Initialize(MemberDto memberDto)
    {
        return new MemberDto(memberDto.Id, memberDto.Status, Initialize(memberDto.Features));
    }

    private static IEnumerable<FeatureDto> Initialize(IEnumerable<FeatureDto> features)
    {
        foreach (var featureId in Enum.GetValues<FeatureId>())
        {
            var feature = features.FirstOrDefault(f => f.Id == featureId);

            yield return feature is not null ? feature : new FeatureDto(featureId, false, false);
        }
    }
}
