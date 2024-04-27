using TrollIt.Application.Shares.Abstractions;
using TrollIt.Application.Shares.Models;
using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;
using TrollIt.Domain.Shares.Infrastructure;

namespace TrollIt.Application.Shares;

internal class SharesService(ISharesRepository sharesRepository, ISharesAcl sharesAcl) : ISharesService
{
    public async Task<SharePolicyResponse> CreateSharePolicyAsync(AppUser user, CreateSharePolicyRequest request, CancellationToken cancellationToken)
    {
        var sharePolicy = sharesAcl.ToDomain(new SharePolicyDto(Guid.NewGuid(), request.Name, [new MemberDto(user.TrollId, ShareStatus.Owner, [
            new FeatureDto(FeatureId.Profile, request.Features.Profile.CanRead, request.Features.Profile.CanRefresh),
            new FeatureDto(FeatureId.View, request.Features.View.CanRead, request.Features.View.CanRefresh),
        ])]));

        await sharesRepository.SaveAsync(sharePolicy, cancellationToken);

        return new SharePolicyResponse(sharePolicy);
    }

    public async Task<SharePolicyResponse?> GetSharePolicyAsync(AppUser user, Guid sharePolicyId, CancellationToken cancellationToken)
    {
        var sharePolicy = await sharesRepository.GetSharePolicyAsync(sharePolicyId, cancellationToken);
        if (sharePolicy == null)
        {
            return null;
        }

        var userMember = sharePolicy.GetMember(user.TrollId);

        return userMember == null || userMember.IsGuest ? null : new SharePolicyResponse(sharePolicy);
    }
}
