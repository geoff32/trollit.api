using TrollIt.Domain.Shares.Abstractions;

namespace TrollIt.Application.Shares.Models;

public record InvitationResponse(Guid Id, string Name, RightResponse Profile, RightResponse View)
{
    public InvitationResponse(IInvitation invitation, IPolicy policy)
        : this(invitation.PolicyId, policy.Name,
            new RightResponse(invitation.Features.First(f => f.Id == FeatureId.Profile)),
            new RightResponse(invitation.Features.First(f => f.Id == FeatureId.View)))
    {

    }
}
