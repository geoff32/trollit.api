
using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;
using TrollIt.Domain.Accounts.Infrastructure;

namespace TrollIt.Domain.Accounts;

internal class Account(Guid id, string login, IPassword password, ITroll troll, IPasswordEncryptor passwordEncryptor) : IAccount
{
    public Guid Id { get; } = id;

    public string Login { get; } = login;

    public IPassword Password { get; } = password;

    public ITroll Troll { get; } = troll;

    public Account(AccountDto accountDto, IEnumerable<byte> encryptedPassword, IPasswordEncryptor passwordEncryptor)
        : this(accountDto.Id, accountDto.Login, new Password(encryptedPassword), new Troll(accountDto.Troll), passwordEncryptor)
    {
    }

    public Account(AccountDto accountDto, string password, IPasswordEncryptor passwordEncryptor)
        : this(accountDto.Id, accountDto.Login, EncryptPassword(accountDto, password, passwordEncryptor), new Troll(accountDto.Troll), passwordEncryptor)
    {
    }

    public bool ValidateCredentials(string password)
        => Password.Equals(new Password(password, passwordEncryptor, Id.ToString()));

    private static Password EncryptPassword(AccountDto accountDto, string password, IPasswordEncryptor passwordEncryptor)
        => new Password(password, passwordEncryptor, accountDto.Id.ToString());
}
