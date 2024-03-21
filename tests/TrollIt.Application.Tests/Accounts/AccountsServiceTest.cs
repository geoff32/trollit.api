using FluentAssertions;
using NSubstitute;
using TrollIt.Application.Accounts.Exceptions;
using TrollIt.Application.Accounts.Models;
using TrollIt.Application.Tests.Accounts.Fixtures;
using TrollIt.Infrastructure;
using TrollIt.Infrastructure.Mountyhall.Errors;

namespace TrollIt.Application.Tests.Accounts;

public class AccountsServiceTest(AccountsFixtureBuilder accountsFixtureBuilder) : IClassFixture<AccountsFixtureBuilder>
{
    private readonly AccountsFixture _accountsFixture = accountsFixtureBuilder.Build();

    [Fact]
    public async Task CreateAccountAsync_ValidAccount_ShouldCreateAccount()
    {
        var trollName = "troll name";
        var request = new CreateAccountRequest("name", "password", 1, "token");

        var trollBestiary = _accountsFixture.MockExistingTroll(request.TrollId);
        trollBestiary.Name.Returns(trollName);
        var account = _accountsFixture.MockNotExistingAccount(request);

        var accountResponse = await _accountsFixture.AccountsService.CreateAccountAsync(request, new CancellationTokenSource().Token);

        _accountsFixture.AssertRefreshProfile(request.TrollId, request.Token);
        _accountsFixture.AssertAccountCreation(account);
        accountResponse.Should().BeEquivalentTo(new AccountResponse(account.Id, request.UserName, new TrollResponse(request.TrollId, trollName)));
    }

    [Fact]
    public async Task CreateAccountAsync_UnknownTroll_ShouldThrowUnknowTrollException()
    {
        var request = new CreateAccountRequest("name", "password", 1, "token");

        _accountsFixture.MockNotExistingTroll(request.TrollId);

        var createAccountAct = () => _accountsFixture.AccountsService.CreateAccountAsync(request, new CancellationTokenSource().Token);

        (await createAccountAct.Should().ThrowAsync<AppException<AccountExceptions>>())
            .Which.Code.Should().Be("TrollUnknown");
        _accountsFixture.AssertNoAccountCreation();
    }

    [Fact]
    public async Task CreateAccountAsync_AccountAlreadyExists_ShouldThrowAccountAlreadyExistsException()
    {
        var request = new CreateAccountRequest("name", "password", 1, "token");

        _accountsFixture.MockExistingAccount(request.UserName);

        var createAccountAct = () => _accountsFixture.AccountsService.CreateAccountAsync(request, new CancellationTokenSource().Token);

        (await createAccountAct.Should().ThrowAsync<AppException<AccountExceptions>>())
            .Which.Code.Should().Be("AccountAlreadyExists");
        _accountsFixture.AssertNoAccountCreation();
    }

    [Theory]
    [InlineData(PublicScriptErrorCodes.Unknown)]
    [InlineData(PublicScriptErrorCodes.IncorrectParameter)]
    [InlineData(PublicScriptErrorCodes.UnknownTroll)]
    [InlineData(PublicScriptErrorCodes.WrongPassword)]
    [InlineData(PublicScriptErrorCodes.Maintenance)]
    [InlineData(PublicScriptErrorCodes.ScriptTemporarilyDisabled)]
    [InlineData(PublicScriptErrorCodes.PnjOrTrollDisabled)]
    internal async Task CreateAccountAsync_RefreshProfileFailed_ShouldThrowException(PublicScriptErrorCodes errorCode)
    {
        var trollName = "troll name";
        var request = new CreateAccountRequest("name", "password", 1, "token");

        var trollBestiary = _accountsFixture.MockExistingTroll(request.TrollId);
        trollBestiary.Name.Returns(trollName);
        var _ = _accountsFixture.MockNotExistingAccount(request);

        _accountsFixture.MockRefreshProfileFailed(request.TrollId, request.Token, errorCode);

        var createAccountAct = () => _accountsFixture.AccountsService.CreateAccountAsync(request, new CancellationTokenSource().Token);

        (await createAccountAct.Should().ThrowAsync<InfrastructureException<PublicScriptErrorCodes>>())
            .Which.Code.Should().Be(Enum.GetName(errorCode));
            
        _accountsFixture.AssertRefreshProfile(request.TrollId, request.Token);
        _accountsFixture.AssertNoAccountCreation();
    }

    [Fact]
    public async Task AuthenticateAsync_ValidLoginAndPassword_ShouldReturnAccount()
    {
        var request = new AuthenticateRequest("login", "password");
        var account = _accountsFixture.MockAuthenticatedAccount(request);

        var response = await _accountsFixture.AccountsService.AuthenticateAsync(request, new CancellationTokenSource().Token);

        response.Should().BeEquivalentTo(new AccountResponse(account.Id, account.Login, new TrollResponse(account.Troll.Id, account.Troll.Name)));
    }

    [Fact]
    public async Task AuthenticateAsync_InvalidLogin_ShouldReturnNull()
    {
        var request = new AuthenticateRequest("login", "password");
        _accountsFixture.MockNotExistingAccount(request.UserName);

        var response = await _accountsFixture.AccountsService.AuthenticateAsync(request, new CancellationTokenSource().Token);

        response.Should().BeNull();
    }

    [Fact]
    public async Task AuthenticateAsync_InvalidPassword_ShouldReturnNull()
    {
        var request = new AuthenticateRequest("login", "password");
        _accountsFixture.MockAccountWithInvalidPassword(request);

        var response = await _accountsFixture.AccountsService.AuthenticateAsync(request, new CancellationTokenSource().Token);

        response.Should().BeNull();
    }

    [Fact]
    public async Task GetAccountAsync_ExistingAccount_ShouldReturnAccount()
    {
        var accountId = Guid.NewGuid();
        _accountsFixture.MockExistingAccount(accountId);

        var response = await _accountsFixture.AccountsService.GetAccountAsync(accountId, new CancellationTokenSource().Token);

        response.Should().NotBeNull();
        response.UserId.Should().Be(accountId);
    }

    [Fact]
    public async Task GetAccountAsync_NotExistingAccount_ShouldThrowAccountNotFound()
    {
        var accountId = Guid.NewGuid();
        _accountsFixture.MockNotExistingAccount(accountId);

        var getAccountAct = () => _accountsFixture.AccountsService.GetAccountAsync(accountId, new CancellationTokenSource().Token);

        (await getAccountAct.Should().ThrowAsync<AppException<AccountExceptions>>())
            .Which.Code.Should().Be("AccountNotFound");
    }
}
