namespace TrollIt.Infrastructure.Tests;

public class EncryptorTest
{
    [Theory]
    [InlineData("password", "salt")]
    [InlineData("p455W0rd", "s4Lt")]
    [InlineData("test", "test")]
    public Task Encrypt_ValueWithSalt_ShouldBeEncrypted(string value, string salt)
    {
        var encryptor = new Encryptor();
        var encryptedValue = encryptor.Encrypt(value, salt);

        return Verify(encryptedValue)
            .UseParameters(value, salt);
    }
}