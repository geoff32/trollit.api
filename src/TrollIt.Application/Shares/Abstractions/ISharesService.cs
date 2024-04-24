using TrollIt.Application.Shares.Models;

namespace TrollIt.Application.Shares.Abstractions;

public interface ISharesService
{
    Task<SharePolicyResponse> CreateSharePolicyAsync(AppUser user, CreateSharePolicyRequest request, CancellationToken cancellationToken);
    Task<SharePolicyResponse?> GetSharePolicyAsync(AppUser user, Guid sharePolicyId, CancellationToken cancellationToken);
}
