using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;

namespace TrollIt.Domain.Shares.Acl.Abstractions
{
    public interface ISharesAcl
    {
        ISharePolicy ToDomain(SharePolicyDto policyDto);
        IUserPolicy ToDomain(int trollId, IEnumerable<ISharePolicy> sharePolicies);
    }
}