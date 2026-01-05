using FluentAssertions;
using NSubstitute;
using TrollIt.Domain.Accounts;
using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;
using TrollIt.Domain.Accounts.Infrastructure;
using TrollIt.Domain.Shares.Abstractions;

namespace TrollIt.Domain.Tests;

public class AccountTest
{
    private readonly IPasswordEncryptor _passwordEncryptor;

    public AccountTest()
    {
        _passwordEncryptor = Substitute.For<IPasswordEncryptor>();
    }

    [Fact]
    public void Create_FromDtoAndEncryptedPassword_ShouldReturnAccount()
    {
        var encryptedPassword = new byte[] { 1 };
        var accountDto = MockAccountDto();

        var account = new Account(accountDto, encryptedPassword);

        account.Should().BeEquivalentTo(new
        {
            accountDto.Id,
            accountDto.Login,
            Password = new Password(encryptedPassword, accountDto.Id.ToString()),
            accountDto.Troll
        });
    }

    [Fact]
    public void Create_FromDtoAndRawPassword_ShouldEncryptPassword()
    {
        var rawPassword = "password";
        var encryptedPassword = new byte[] { 1 };
        var accountDto = MockAccountDto();
        var salt = accountDto.Id.ToString();
        _passwordEncryptor.Encrypt(rawPassword, salt).Returns(encryptedPassword);

        var account = new Account(accountDto, rawPassword, _passwordEncryptor);

        _passwordEncryptor.Received(1).Encrypt(rawPassword, salt);
        account.Should().BeEquivalentTo(new
        {
            accountDto.Id,
            accountDto.Login,
            Password = new Password(encryptedPassword, salt),
            accountDto.Troll
        });
    }

    [Fact]
    public void Create_FromAccountDtoAndRawPassword_ShouldEncryptPasswordWithAccountIdAsSalt()
    {
        var accountDto = MockAccountDto();
        var password = "password";
        _ = new Account(accountDto, password, _passwordEncryptor);

        _passwordEncryptor.Received(1).Encrypt(password, accountDto.Id.ToString());
    }

    [Fact]
    public void ValidateCredentials_WithSamePassword_ShouldBeTrue()
    {
        var encryptedPassword = new byte[] { 1 };
        var password = Substitute.For<IPassword>();
        var account = new Account(Guid.NewGuid(), "Login", password, Substitute.For<ITroll>());

        password.Equals(Arg.Any<IPassword>()).Returns(true);

        var isPasswordValid = account.ValidateCredentials(encryptedPassword);

        password.Received(1).Equals(Arg.Any<Password>());
        isPasswordValid.Should().BeTrue();
    }

    [Fact]
    public void ValidateCredentials_WithDifferentPassword_ShouldBeFalse()
    {
        var encryptedPassword = new byte[] { 1 };
        var password = Substitute.For<IPassword>();
        var account = new Account(Guid.NewGuid(), "Login", password, Substitute.For<ITroll>());

        password.Equals(Arg.Any<IPassword>()).Returns(false);

        var isPasswordValid = account.ValidateCredentials(encryptedPassword);

        password.Received(1).Equals(Arg.Any<Password>());
        isPasswordValid.Should().BeFalse();
    }


    [Fact]
    public void GetSalt_FromAccount_ShouldBeAccountId()
    {
        var account = new Account(Guid.NewGuid(), "Login", Substitute.For<IPassword>(), Substitute.For<ITroll>());

        var salt = account.GetSalt();

        salt.Should().Be(account.Id.ToString());
    }
    private static AccountDto MockAccountDto() => new(Guid.NewGuid(), "Login", new TrollDto(1, "Name", "Token"));
}
