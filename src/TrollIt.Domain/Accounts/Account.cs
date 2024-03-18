
using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;
using TrollIt.Domain.Accounts.Infrastructure;

namespace TrollIt.Domain.Accounts;

internal record Account(Guid Id, string Login, IPassword Password, ITroll Troll) : IAccount
{

    public Account(AccountDto accountDto, IEnumerable<byte> encryptedPassword)
        : this(accountDto.Id, accountDto.Login, new Password(encryptedPassword, GetSalt(accountDto)), new Troll(accountDto.Troll))
    {
    }

    public Account(AccountDto accountDto, string password, IPasswordEncryptor passwordEncryptor)
        : this(accountDto.Id, accountDto.Login, EncryptPassword(accountDto, password, passwordEncryptor), new Troll(accountDto.Troll))
    {
    }

    public bool ValidateCredentials(IEnumerable<byte> hashedPassword)
        => Password.Equals(new Password(hashedPassword, GetSalt()));

    public string GetSalt() => Id.ToString();

    private static Password EncryptPassword(AccountDto accountDto, string password, IPasswordEncryptor passwordEncryptor) =>
        new(password, GetSalt(accountDto), passwordEncryptor);

    private static string GetSalt(AccountDto accountDto) => accountDto.Id.ToString();
}
