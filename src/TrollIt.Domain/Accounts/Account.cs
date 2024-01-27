
using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;
using TrollIt.Domain.Accounts.Infrastructure.Abstractions;

namespace TrollIt.Domain.Accounts;

internal class Account(Guid id, string login, IPassword password, ITroll troll) : IAccount
{
    public Guid Id { get; } = id;

    public string Login { get; } = login;

    public IPassword Password { get; } = password;

    public ITroll Troll { get; } = troll;

    public Account(AccountDto accountDto, IEnumerable<byte> encryptedPassword)
        : this(accountDto.Id, accountDto.Login, new Password(encryptedPassword), new Troll(accountDto.Troll))
    {
    }

    public Account(AccountDto accountDto, string password, IPasswordEncryptor passwordEncryptor)
        : this(accountDto.Id, accountDto.Login, EncryptPassword(accountDto, password, passwordEncryptor), new Troll(accountDto.Troll))
    {
    }

    private static Password EncryptPassword(AccountDto accountDto, string password, IPasswordEncryptor passwordEncryptor)
        => new Password(password, passwordEncryptor, accountDto.Id.ToString());
}
