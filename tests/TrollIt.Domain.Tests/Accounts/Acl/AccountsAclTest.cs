using FluentAssertions;
using NSubstitute;
using TrollIt.Domain.Accounts;
using TrollIt.Domain.Accounts.Acl;
using TrollIt.Domain.Accounts.Acl.Models;
using TrollIt.Domain.Accounts.Infrastructure;

namespace TrollIt.Domain.Tests;

public class AccountsAclTest
{
    private readonly IPasswordEncryptor _passwordEncryptor;

    public AccountsAclTest()
    {
        _passwordEncryptor = Substitute.For<IPasswordEncryptor>();
    }

    [Fact]
    public void ToDomain_AccountWithRawPassword_ShouldReturnAccount()
    {
        var accountDto = new AccountDto(Guid.NewGuid(), "Login", new TrollDto(1, "Name", "Token"));
        var accountsAcl = new AccountsAcl(_passwordEncryptor);
        var encryptedPassword = new byte[] { 1 };
        var password = "password";

        _passwordEncryptor.Encrypt(password, Arg.Any<string>()).Returns(encryptedPassword);

        var account = accountsAcl.ToDomain(accountDto, password);

        account.Should().BeEquivalentTo(new
        {
            accountDto.Id,
            accountDto.Login,
            accountDto.Troll,
            Password = new Password(encryptedPassword, account.Id.ToString())
        });
    }

    [Fact]
    public void ToDomain_AccountWithEncryptedPassword_ShouldReturnAccount()
    {
        var accountDto = new AccountDto(Guid.NewGuid(), "Login", new TrollDto(1, "Name", "Token"));
        var accountsAcl = new AccountsAcl(_passwordEncryptor);
        var encryptedPassword = new byte[] { 1 };

        var account = accountsAcl.ToDomain(accountDto, encryptedPassword);

        account.Should().BeEquivalentTo(new
        {
            accountDto.Id,
            accountDto.Login,
            accountDto.Troll,
            Password = new Password(encryptedPassword, account.Id.ToString())
        });
    }
}
