namespace TrollIt.Domain.Accounts.Infrastructure;

public interface IPasswordEncryptor
{
    IEnumerable<byte> Encrypt(string password, string salt);
}
