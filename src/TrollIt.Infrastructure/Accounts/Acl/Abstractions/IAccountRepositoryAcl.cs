using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Infrastructure.Accounts.Models;

namespace TrollIt.Infrastructure.Accounts.Acl.Abstractions;

internal interface IAccountRepositoryAcl
{
    IAccount? ToDomain(Account? account);
}
