using TrollIt.Domain.Shares.Abstractions;

namespace TrollIt.Domain.Shares.Infrastructure;

public interface ISharesRepository
{
    Task<ISharePolicy?> GetSharePolicyAsync(Guid sharePolicyId, CancellationToken cancellationToken);
    Task SaveAsync(ISharePolicy sharePolicy, CancellationToken cancellationToken);
}
