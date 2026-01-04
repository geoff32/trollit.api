using TrollIt.Infrastructure.Shares.Models;

namespace TrollIt.Infrastructure.Shares.Abstractions;

internal interface IInternalSharesRepository
{
    public Task<IEnumerable<Policy>> InternalGetTrollPoliciesAsync(int trollId, CancellationToken cancellationToken);
}
