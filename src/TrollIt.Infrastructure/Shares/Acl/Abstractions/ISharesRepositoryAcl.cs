using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Infrastructure.Shares.Models;

namespace TrollIt.Infrastructure.Shares.Acl.Abstractions;

internal interface ISharesRepositoryAcl
{
    SharePolicy ToDataModel(ISharePolicy sharePolicy);
    ISharePolicy? ToDomain(SharePolicy? data);
    IEnumerable<ISharePolicy> ToDomain(IEnumerable<SharePolicy> data);
}
