using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Acl.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;
using TrollIt.Infrastructure.Accounts.Acl.Abstractions;
using TrollIt.Infrastructure.Accounts.Models;

namespace TrollIt.Infrastructure.Accounts.Acl;

internal class AccountsRepositoryAcl(IAccountsAcl accountAcl) : IAccountsRepositoryAcl
{
    public IAccount? ToDomain(Account? account)
        => account == null ? null : accountAcl.ToDomain(
            new AccountDto(account.Id, account.Login, new TrollDto(account.TrollId, account.TrollName, account.ScriptToken)),
            account.Password);
}
