using System.Security.Cryptography;
using System.Text;
using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Infrastructure;

namespace TrollIt.Infrastructure;

internal class Encryptor : IPasswordEncryptor
{
    public IEnumerable<byte> Encrypt(string password, string salt)
    {
        using var sha256 = new HMACSHA256(GetBytes(salt));
        return sha256.ComputeHash(GetBytes(password));
    }

    public IEnumerable<byte> HashPassword(string password, IAccount account)
        => Encrypt(password, account.GetSalt());

    private static byte[] GetBytes(string input) => Encoding.UTF8.GetBytes(input);
}
