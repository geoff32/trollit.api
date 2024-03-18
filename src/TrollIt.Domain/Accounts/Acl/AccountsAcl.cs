using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Acl.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;
using TrollIt.Domain.Accounts.Infrastructure;

namespace TrollIt.Domain.Accounts.Acl;

internal class AccountsAcl(IPasswordEncryptor passwordEncryptor) : IAccountsAcl
{
    public IAccount ToDomain(AccountDto accountDto, IEnumerable<byte> encryptedPassword)
        => new Account(accountDto, encryptedPassword);

    public IAccount ToDomain(AccountDto accountDto, string password)
        => new Account(accountDto, password, passwordEncryptor);
}
