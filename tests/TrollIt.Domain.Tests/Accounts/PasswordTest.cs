using FluentAssertions;
using NSubstitute;
using TrollIt.Domain.Accounts;
using TrollIt.Domain.Accounts.Exceptions;
using TrollIt.Domain.Accounts.Infrastructure;

namespace TrollIt.Domain.Tests;

public class PasswordTest
{
    [Fact]
    public void Password_LessThan5Characters_ShouldThrowInsecurePassword()
    {
        var passwordEncryptor = Substitute.For<IPasswordEncryptor>();
        var createPasswordAct = () => new Password("test", "salt", passwordEncryptor);

        createPasswordAct.Should().Throw<DomainException<AccountsException>>()
            .Which.Code.Should().Be("InsecurePassword");
    }

    [Fact]
    public void Password_FromStringValue_ShouldEncryptValue()
    {
        var passwordValue = "password";
        var salt = "salt";
        var encryptedPassword = new byte[] { 1 };

        var passwordEncryptor = Substitute.For<IPasswordEncryptor>();
        passwordEncryptor.Encrypt(passwordValue, salt).Returns(encryptedPassword.Clone());

        var password = new Password(passwordValue, salt, passwordEncryptor);

        passwordEncryptor.Received(1).Encrypt(passwordValue, salt);
        password.Value.Should().BeEquivalentTo(encryptedPassword);
    }

    [Fact]
    public void Equals_WithSameEncryptedValueAndSalt_ShouldReturnTrue()
    {
        var encryptedPassword = new byte[] { 1 };
        var salt = "salt";
        var password = new Password(encryptedPassword, salt);
        var otherPassword = new Password(encryptedPassword, salt);

        var isEquals = password.Equals(otherPassword);

        isEquals.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentEncryptedValue_ShouldReturnFalse()
    {
        var salt = "salt";
        var password = new Password([1], salt);
        var otherPassword = new Password([1, 2], salt);

        var isEquals = password.Equals(otherPassword);

        isEquals.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentSalt_ShouldReturnFalse()
    {
        var encryptedPassword = new byte[] { 1 };
        var password = new Password(encryptedPassword, "salt");
        var otherPassword = new Password(encryptedPassword, "salt2");

        var isEquals = password.Equals(otherPassword);

        isEquals.Should().BeFalse();
    }
}
