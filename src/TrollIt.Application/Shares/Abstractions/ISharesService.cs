using TrollIt.Application.Shares.Models;

namespace TrollIt.Application.Shares.Abstractions;

public interface ISharesService
{
    Task<PolicyResponse> CreatePolicyAsync(AppUser user, CreatePolicyRequest request, CancellationToken cancellationToken);
    Task<PolicyResponse?> GetPolicyAsync(AppUser user, Guid policyId, CancellationToken cancellationToken);
    IAsyncEnumerable<InvitationResponse> GetUserInvitationsAsync(AppUser getAppUserFromClaims, CancellationToken cancellationToken);
    Task<PolicyResponse?> SendAnswerAsync(AppUser user, Guid invitationId, SendAnswerRequest request, CancellationToken cancellationToken);
    Task<InvitationResponse?> SendInvitationAsync(AppUser getAppUserFromClaims, Guid policyId, SendInvitationRequest request, CancellationToken cancellationToken);
}
