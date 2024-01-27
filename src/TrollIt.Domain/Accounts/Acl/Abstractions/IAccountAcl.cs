using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;

namespace TrollIt.Domain.Accounts.Acl.Abstractions;

public interface IAccountAcl
{
    IAccount ToAccount(AccountDto accountDto, IEnumerable<byte> encryptedPassword);
    IAccount ToAccount(AccountDto accountDto, string password);
}
