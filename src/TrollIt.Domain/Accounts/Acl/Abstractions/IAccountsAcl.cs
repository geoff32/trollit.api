using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;

namespace TrollIt.Domain.Accounts.Acl.Abstractions;

public interface IAccountsAcl
{
    IAccount ToDomain(AccountDto accountDto, IEnumerable<byte> encryptedPassword);
    IAccount ToDomain(AccountDto accountDto, string password);
}
