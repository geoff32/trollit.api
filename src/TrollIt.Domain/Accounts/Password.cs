using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Exceptions;
using TrollIt.Domain.Accounts.Infrastructure;

namespace TrollIt.Domain.Accounts;

internal record Password(IEnumerable<byte> value) : IPassword, IEquatable<IPassword>
{
    public Password(string password, IPasswordEncryptor passwordEncryptor, string salt)
        : this(passwordEncryptor.Encrypt(password, salt))
    {
        if (password.Length <= 5)
        {
            throw new DomainException<AccountsException>(AccountsException.InsecurePassword);
        }
    }

    public IEnumerable<byte> Value { get; } = value;

    public bool Equals(IPassword? other) => other != null && Value.SequenceEqual(other.Value);

    public override int GetHashCode() => Value.GetHashCode();
}
