using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using TrollIt.Application.Accounts;
using TrollIt.Application.Accounts.Models;
using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Acl.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;
using TrollIt.Domain.Accounts.Infrastructure;
using TrollIt.Domain.Bestiaries.Infrastructure;
using TrollIt.Domain.Profiles.Abstractions;
using TrollIt.Domain.Profiles.Infrastructure;
using TrollIt.Infrastructure;

namespace TrollIt.Application.Tests.Accounts.Fixtures;

public class AccountsFixture
{
    private readonly IAccountsAcl _accountsAcl;
    private readonly IPasswordEncryptor _passwordEncryptor;
    private readonly IAccountsRepository _accountsRepository;
    private readonly ITrollBestiary _trollBestiary;
    private readonly IProfilesRepository _profilesRepository;

    internal AccountsService AccountsService { get; }

    public AccountsFixture()
    {
        _accountsAcl = Substitute.For<IAccountsAcl>();
        _passwordEncryptor = Substitute.For<IPasswordEncryptor>();
        _accountsRepository = Substitute.For<IAccountsRepository>();
        _trollBestiary = Substitute.For<ITrollBestiary>();
        _profilesRepository = Substitute.For<IProfilesRepository>();

        AccountsService = new AccountsService(_accountsAcl, _passwordEncryptor, _accountsRepository, _trollBestiary, _profilesRepository);
    }

    public Domain.Bestiaries.Abstractions.ITroll MockExistingTroll(int trollId)
    {
        var troll = Substitute.For<Domain.Bestiaries.Abstractions.ITroll>();
        _trollBestiary.GetTrollAsync(trollId).Returns(troll);

        return troll;
    }

    public void MockNotExistingTroll(int trollId) => _trollBestiary.GetTrollAsync(trollId).ReturnsNull();

    public IAccount MockExistingAccount(Guid id)
    {
        var account = Substitute.For<IAccount>();
        account.Id.Returns(id);
        _accountsRepository.GetAccountAsync(id, Arg.Any<CancellationToken>()).Returns(account);

        return account;
    }

    public IAccount MockExistingAccount(string userName)
    {
        var account = Substitute.For<IAccount>();
        account.Login.Returns(userName);
        _accountsRepository.GetAccountByLoginAsync(userName, Arg.Any<CancellationToken>()).Returns(account);

        return account;
    }

    public IAccount MockNotExistingAccount(Guid id)
    {
        var account = Substitute.For<IAccount>();
        account.Id.Returns(id);
        _accountsRepository.GetAccountAsync(id, Arg.Any<CancellationToken>()).ReturnsNull();

        return account;
    }

    public IAccount MockNotExistingAccount(string userName)
    {
        var account = Substitute.For<IAccount>();
        account.Login.Returns(userName);
        _accountsRepository.GetAccountByLoginAsync(userName, Arg.Any<CancellationToken>()).ReturnsNull();

        return account;
    }

    public IAccount MockNotExistingAccount(CreateAccountRequest createAccountRequest)
    {
        var account = MockNotExistingAccount(createAccountRequest.UserName);
        
        var troll = Substitute.For<ITroll>();
        account.Password.Returns(Substitute.For<IPassword>());
        account.Troll.Returns(troll);
        troll.Id.Returns(createAccountRequest.TrollId);
        troll.ScriptToken.Returns(createAccountRequest.Token);

        _accountsAcl
                .ToDomain
                (
                    Arg.Is<AccountDto>(a => a.Login == createAccountRequest.UserName
                        && a.Troll.Id == createAccountRequest.TrollId
                        && a.Troll.ScriptToken == createAccountRequest.Token),
                    createAccountRequest.Password
                )
                .Returns(args =>
                {
                    var accountDto = args.ArgAt<AccountDto>(0);
                    account.Id.Returns(accountDto.Id);
                    troll.Name.Returns(accountDto.Troll.Name);
                    return account;
                });

        return account;
    }

    public IAccount MockAuthenticatedAccount(AuthenticateRequest authenticateRequest)
    {
        var salt = "salt";
        var hashedPassword = new byte[] { 1 };
        var password = Substitute.For<IPassword>();
        password.Value.Returns(hashedPassword);
        password.Salt.Returns(salt);
        _passwordEncryptor.Encrypt(authenticateRequest.Password, salt).Returns(hashedPassword);

        var account = MockExistingAccount(authenticateRequest.UserName);
        account.Password.Returns(password);
        account.ValidateCredentials(hashedPassword).Returns(true);
        account.GetSalt().Returns(salt);

        return account;
    }

    public IAccount MockAccountWithInvalidPassword(AuthenticateRequest authenticateRequest)
    {
        var salt = "salt";
        var hashedPassword = new byte[] { 1 };
        var password = Substitute.For<IPassword>();
        password.Value.Returns(hashedPassword);
        password.Salt.Returns(salt);
        _passwordEncryptor.Encrypt(authenticateRequest.Password, salt).Returns(hashedPassword);

        var account = MockExistingAccount(authenticateRequest.UserName);
        account.Password.Returns(password);
        account.ValidateCredentials(hashedPassword).Returns(false);
        account.GetSalt().Returns(salt);

        return account;
    }

    public InfrastructureException<TErrorCategory> MockRefreshProfileFailed<TErrorCategory>(int trollId, string token, TErrorCategory errorCode)
        where TErrorCategory : struct, Enum
    {
        var exception = new InfrastructureException<TErrorCategory>(errorCode);
        _profilesRepository
            .RefreshProfileAsync(trollId, token, Arg.Any<CancellationToken>())
            .Throws(exception);

        return exception;
    }

    public void AssertRefreshProfile(int trollId, string token)
        => _profilesRepository.Received(1)
            .RefreshProfileAsync(trollId, token, Arg.Any<CancellationToken>());

    public void AssertAccountCreation(IAccount account)
        => _accountsRepository.Received(1)
            .CreateAccountAsync(account, Arg.Any<CancellationToken>());

    public void AssertNoAccountCreation()
        => _accountsRepository.DidNotReceive()
            .CreateAccountAsync(Arg.Any<IAccount>(), Arg.Any<CancellationToken>());
}
