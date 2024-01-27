using System.Security.Cryptography;
using System.Text;
using TrollIt.Domain.Accounts.Infrastructure.Abstractions;

namespace TrollIt.Infrastructure;

public class Encryptor : IPasswordEncryptor
{
    public IEnumerable<byte> Encrypt(string password, string salt)
    {
        using (var sha256 = new HMACSHA256(GetBytes(salt)))
        {
            return sha256.ComputeHash(GetBytes(password));
        }
    }

    private byte[] GetBytes(string input) => Encoding.UTF8.GetBytes(input);
}
