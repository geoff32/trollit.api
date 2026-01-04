using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Infrastructure.Accounts.Models;
using TrollIt.Infrastructure.Shares.Models;

namespace TrollIt.Infrastructure.Accounts.Acl.Abstractions;

internal interface IAccountsRepositoryAcl
{
    IAccount? ToDomain(Account? account);
    IAccountPolicy ToDomain(int trollId, IEnumerable<Policy> policies);
}
