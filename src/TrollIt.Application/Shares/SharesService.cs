using System.Runtime.CompilerServices;
using TrollIt.Application.Shares.Abstractions;
using TrollIt.Application.Shares.Models;
using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;
using TrollIt.Domain.Shares.Infrastructure;

namespace TrollIt.Application.Shares;

internal class SharesService(ISharesRepository sharesRepository, ISharesAcl sharesAcl) : ISharesService
{
    public async Task<PolicyResponse> CreatePolicyAsync(AppUser user, CreatePolicyRequest request, CancellationToken cancellationToken)
    {
        var policy = sharesAcl.ToDomain(new PolicyDto(Guid.NewGuid(), request.Name, [new MemberDto(user.TrollId, PolicyStatus.Owner, ToFeatures(request.Features))]));

        await sharesRepository.SaveAsync(policy, cancellationToken);

        return new PolicyResponse(policy);
    }

    public async Task<PolicyResponse?> GetPolicyAsync(AppUser user, Guid policyId, CancellationToken cancellationToken)
    {
        var policy = await sharesRepository.GetPolicyAsync(policyId, cancellationToken);
        if (policy == null)
        {
            return null;
        }

        var userMember = policy.GetMember(user.TrollId);

        return userMember == null || userMember.IsGuest ? null : new PolicyResponse(policy);
    }

    public async IAsyncEnumerable<InvitationResponse> GetUserInvitationsAsync(AppUser getAppUserFromClaims,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var trollId = getAppUserFromClaims.TrollId;
        var policies = await sharesRepository.GetTrollPoliciesAsync(trollId, cancellationToken);
        foreach (var policy in policies.Where(p => p.GetMember(trollId)?.IsGuest ?? false))
        {
            var invitation = policy.GetInvitation(trollId);
            yield return new InvitationResponse(invitation, policy);
        }
    }

    public async Task<PolicyResponse?> SendAnswerAsync(AppUser user, Guid invitationId, SendAnswerRequest request,
        CancellationToken cancellationToken)
    {
        var policy = await sharesRepository.GetPolicyAsync(invitationId, cancellationToken);
        if (policy == null)
        {
            return null;
        }
        
        var invitation = policy.GetInvitation(user.TrollId);
        if (request.Accept)
        {
            policy.AcceptInvitation(invitation);
        }
        else
        {
            policy.RemoveInvitation(invitation);
        }
        await sharesRepository.SaveAsync(policy, cancellationToken);

        return new PolicyResponse(policy);
    }

    public async Task<InvitationResponse?> SendInvitationAsync(AppUser getAppUserFromClaims, Guid policyId, SendInvitationRequest request,
        CancellationToken cancellationToken)
    {
        var policy = await sharesRepository.GetPolicyAsync(policyId, cancellationToken);
        if (policy == null)
        {
            return null;
        }
        
        var invitation = sharesAcl.ToDomain(new InvitationDto(policyId, request.TrollId, ToFeatures(request.Features)));
        policy.AddInvitation(invitation);
        await sharesRepository.SaveAsync(policy, cancellationToken);

        return new InvitationResponse(invitation, policy);
    }

    private static IReadOnlyCollection<FeatureDto> ToFeatures(FeatureSettingsRequest request)
    {
        return [
            new FeatureDto(FeatureId.Profile, request.Profile.CanRead, request.Profile.CanRefresh),
            new FeatureDto(FeatureId.View, request.View.CanRead, request.View.CanRefresh),
        ];
    }
}
