using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;

namespace TrollIt.Domain.Shares.Acl;

public class SharesAcl : ISharesAcl
{
    public IPolicy ToDomain(PolicyDto policyDto) => new Policy(Initialize(policyDto));
    public IInvitation ToDomain(InvitationDto invitationDto) => new Invitation(Initialize(invitationDto));

    private static PolicyDto Initialize(PolicyDto policyDto) =>
        policyDto with { Members = policyDto.Members.Select(Initialize) };

    private static InvitationDto Initialize(InvitationDto invitationDto) =>
        invitationDto with { Features = Initialize(invitationDto.Features) };

    private static MemberDto Initialize(MemberDto memberDto) =>
        memberDto with { Features = Initialize(memberDto.Features) };

    private static IReadOnlyCollection<FeatureDto> Initialize(IReadOnlyCollection<FeatureDto> features)
    {
        return Enum.GetValues<FeatureId>().Select(featureId =>
            features.FirstOrDefault(f => f.Id == featureId) ?? new FeatureDto(featureId, false, false))
            .ToArray();
    }
}
