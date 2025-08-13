using TrollIt.Infrastructure.Shares.Models;

namespace TrollIt.Infrastructure.Shares.Abstractions;

internal interface IInternalSharesRepository
{
    Task<IEnumerable<SharePolicy>> InternalGetTrollPoliciesAsync(int trollId, CancellationToken cancellationToken);
}
