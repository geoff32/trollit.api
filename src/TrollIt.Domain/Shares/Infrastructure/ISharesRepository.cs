using TrollIt.Domain.Shares.Abstractions;

namespace TrollIt.Domain.Shares.Infrastructure;

public interface ISharesRepository
{
    Task<IPolicy?> GetPolicyAsync(Guid policyId, CancellationToken cancellationToken);
    Task SaveAsync(IPolicy policy, CancellationToken cancellationToken);
    Task<IEnumerable<IPolicy>> GetTrollPoliciesAsync(int trollId, CancellationToken cancellationToken);
}
