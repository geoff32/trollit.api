using System.Security.Cryptography;
using System.Text;
using TrollIt.Domain.Accounts.Infrastructure;

namespace TrollIt.Infrastructure;

internal class Encryptor : IPasswordEncryptor
{
    public IEnumerable<byte> Encrypt(string value, string salt)
    {
        using var sha256 = new HMACSHA256(GetBytes(salt));
        return sha256.ComputeHash(GetBytes(value));
    }

    private static byte[] GetBytes(string input) => Encoding.UTF8.GetBytes(input);
}
