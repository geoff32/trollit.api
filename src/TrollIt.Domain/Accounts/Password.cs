using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Exceptions;
using TrollIt.Domain.Accounts.Infrastructure.Abstractions;

namespace TrollIt.Domain.Accounts;

internal record class Password(IEnumerable<byte> Value) : IPassword
{
    public Password(string password, IPasswordEncryptor passwordEncryptor, string salt)
        : this(passwordEncryptor.Encrypt(password, salt))
    {
        if (password.Length <= 5)
        {
            throw new DomainException<AccountsException>(AccountsException.InsecurePassword);
        }
    }
}
