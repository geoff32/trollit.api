using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;

namespace TrollIt.Domain.Shares.Acl;

public class SharesAcl : ISharesAcl
{
    public ISharePolicy ToDomain(SharePolicyDto policyDto) => new SharePolicy(Initialize(policyDto));

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
