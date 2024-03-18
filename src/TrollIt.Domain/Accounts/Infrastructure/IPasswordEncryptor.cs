using TrollIt.Domain.Accounts.Abstractions;

namespace TrollIt.Domain.Accounts.Infrastructure;

public interface IPasswordEncryptor
{
    IEnumerable<byte> Encrypt(string password, string salt);
    IEnumerable<byte> HashPassword(string password, IAccount account);
}
