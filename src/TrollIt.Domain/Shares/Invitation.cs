using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;

namespace TrollIt.Domain.Shares;

internal class Invitation(Guid policyId, int trollId, IReadOnlyCollection<IFeature> features) : IInvitation
{
    public Invitation(InvitationDto invitationDto)
        : this(
            invitationDto.PolicyId,
            invitationDto.TrollId,
            invitationDto.Features.Select(f => new Feature(f)).ToArray())
    {
    }

    public Guid PolicyId { get; } = policyId;
    public int TrollId { get; } = trollId;
    public IReadOnlyCollection<IFeature> Features { get; } = features;
}