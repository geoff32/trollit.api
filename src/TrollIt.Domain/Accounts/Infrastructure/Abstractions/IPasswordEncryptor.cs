using TrollIt.Domain.Accounts.Abstractions;

namespace TrollIt.Domain.Accounts.Infrastructure.Abstractions;

public interface IPasswordEncryptor
{
    IEnumerable<byte> Encrypt(string password, string salt);
}
