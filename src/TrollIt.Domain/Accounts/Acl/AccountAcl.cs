using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Acl.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;
using TrollIt.Domain.Accounts.Infrastructure.Abstractions;

namespace TrollIt.Domain.Accounts.Acl;

internal class AccountAcl(IPasswordEncryptor passwordEncryptor) : IAccountAcl
{
    public IAccount ToAccount(AccountDto accountDto, IEnumerable<byte> encryptedPassword)
        => new Account(accountDto, encryptedPassword, passwordEncryptor);

    public IAccount ToAccount(AccountDto accountDto, string password)
        => new Account(accountDto, password, passwordEncryptor);
}
