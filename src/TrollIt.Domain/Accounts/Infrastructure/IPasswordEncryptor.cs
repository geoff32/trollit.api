namespace TrollIt.Domain.Accounts.Infrastructure;

public interface IPasswordEncryptor
{
    IEnumerable<byte> Encrypt(string value, string salt);
}
