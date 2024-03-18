using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Exceptions;
using TrollIt.Domain.Accounts.Infrastructure;

namespace TrollIt.Domain.Accounts;

internal sealed record Password(IEnumerable<byte> Value, string Salt) : IPassword, IEquatable<Password>
{
    public Password(string password, string salt, IPasswordEncryptor passwordEncryptor)
        : this(passwordEncryptor.Encrypt(password, salt), salt)
    {
        if (password.Length <= 5)
        {
            throw new DomainException<AccountsException>(AccountsException.InsecurePassword);
        }
    }

    public bool Equals(IPassword? other) => other != null && Salt.Equals(other.Salt) && Value.SequenceEqual(other.Value);
    public bool Equals(Password? other) => Equals(other as IPassword);

    public override int GetHashCode() => HashCode.Combine(Value, Salt);
}
