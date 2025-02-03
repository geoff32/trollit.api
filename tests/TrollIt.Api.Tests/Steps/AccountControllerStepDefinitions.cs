using System.Net.Http.Json;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Reqnroll;
using TrollIt.Api.Tests.Context;
using TrollIt.Application;
using TrollIt.Application.Accounts.Models;
using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;
using TrollIt.Domain.Profiles.Abstractions;
using TrollIt.Infrastructure;
using TrollIt.Infrastructure.Mountyhall.Errors;

namespace TrollIt.Api.Tests.Steps;

[Binding]
public sealed class AccountControllerStepDefinitions(WebApiContext webApiContext, VerifyContext verifyContext, ScenarioContext scenarioContext)
{
    [Given(@"The name of troll (\d+) is (.*)")]
    public void GivenTheNameOfTrollIs(int trollId, string trollName)
    {
        var troll = Substitute.For<Domain.Bestiaries.Abstractions.ITroll>();
        troll.Name.Returns(trollName);
        webApiContext.TrollBestiary.GetTrollAsync(trollId, Arg.Any<CancellationToken>()).Returns(troll);
    }

    [Then(@"The account response should be ok")]
    public async Task ThenTheAccountResponseShouldBeOk()
    {
        var response = scenarioContext.Get<HttpResponseMessage>();
        WebApiContext.EnsureResponseInitialized(response);

        var verifySettings = verifyContext.Settings;
        verifySettings.IgnoreMember<AccountResponse>(accountResponse => accountResponse.UserId);
        await response.Should().BeOk<AccountResponse>(verifySettings);
    }

    #region Account Creation
    
    [Given(@"I have a new valid account request")]
    public void GivenIHaveANewValidAccountRequest()
    {
        scenarioContext.Set(new CreateAccountRequest("testUserName", "testPassword", 1, "testToken"));
    }

    [Given(@"I have a new account request")]
    public void GivenIHaveANewAccountRequest(Table table)
    {
        scenarioContext.Set(table.CreateInstance<CreateAccountRequest>());
    }

    [Given(@"I have a new account request with password too short")]
    public void GivenIHaveANewAccountRequestWithPasswordTooShort()
    {
        scenarioContext.Set(new CreateAccountRequest("testUserName", "pass", 1, "testToken"));
    }

    [Given(@"The account exists")]
    public void GivenTheAccountExists()
    {
        var createAccountRequest = scenarioContext.Get<CreateAccountRequest>();
        WebApiContext.EnsureRequestInitialized(createAccountRequest);
        
        webApiContext.AccountsRepository.GetAccountByLoginAsync(createAccountRequest.UserName, Arg.Any<CancellationToken>())
            .Returns(Substitute.For<IAccount>());
    }

    [Given(@"The account does not exist")]
    public void GivenTheAccountDoesNotExist()
    {
        var createAccountRequest = scenarioContext.Get<CreateAccountRequest>();
        WebApiContext.EnsureRequestInitialized(createAccountRequest);
        
        webApiContext.AccountsRepository.GetAccountByLoginAsync(createAccountRequest.UserName, Arg.Any<CancellationToken>()).ReturnsNull();
    }

    [Given(@"The troll exists")]
    public void GivenTheTrollExists()
    {
        var createAccountRequest = scenarioContext.Get<CreateAccountRequest>();
        WebApiContext.EnsureRequestInitialized(createAccountRequest);
        
        webApiContext.TrollBestiary.GetTrollAsync(createAccountRequest.TrollId, Arg.Any<CancellationToken>())
            .Returns(Substitute.For<Domain.Bestiaries.Abstractions.ITroll>());
    }

    [Given(@"The troll does not exist")]
    public void GivenTheTrollDoesNotExist()
    {
        var createAccountRequest = scenarioContext.Get<CreateAccountRequest>();
        WebApiContext.EnsureRequestInitialized(createAccountRequest);
        
        webApiContext.TrollBestiary.GetTrollAsync(createAccountRequest.TrollId, Arg.Any<CancellationToken>()).ReturnsNull();
    }

    [Given(@"The account token is valid")]
    public void GivenTheAccountTokenIsValid()
    {
        var createAccountRequest = scenarioContext.Get<CreateAccountRequest>();
        WebApiContext.EnsureRequestInitialized(createAccountRequest);
        
        webApiContext.ProfilesRepository.RefreshProfileAsync(createAccountRequest.TrollId, createAccountRequest.Token, Arg.Any<CancellationToken>())
            .Returns(Substitute.For<IProfile>());
    }

    [Given(@"The account token is invalid")]
    public void GivenTheAccountTokenIsInvalid()
    {
        var createAccountRequest = scenarioContext.Get<CreateAccountRequest>();
        WebApiContext.EnsureRequestInitialized(createAccountRequest);
        
        webApiContext.ProfilesRepository.RefreshProfileAsync(createAccountRequest.TrollId, createAccountRequest.Token, Arg.Any<CancellationToken>())
            .Throws(new InfrastructureException<PublicScriptErrorCodes>(PublicScriptErrorCodes.WrongPassword));
    }

    [When(@"I create the account")]
    public async Task WhenICreateTheAccount()
    {
        var createAccountRequest = scenarioContext.Get<CreateAccountRequest>();
        WebApiContext.EnsureRequestInitialized(createAccountRequest);

        scenarioContext.Set(await webApiContext.Client.PostAsJsonAsync("api/account", createAccountRequest));
    }

    [Then(@"The account should be created")]
    public void ThenTheAccountShouldBeCreated()
    {
        var createAccountRequest = scenarioContext.Get<CreateAccountRequest>();
        WebApiContext.EnsureRequestInitialized(createAccountRequest);

        webApiContext.AccountsRepository.Received(1).CreateAccountAsync(Arg.Is<IAccount>(a => a.Login == createAccountRequest.UserName), Arg.Any<CancellationToken>());
    }

    [Then(@"The account should not be created")]
    public void ThenTheAccountShouldNotBeCreated()
    {
        var createAccountRequest = scenarioContext.Get<CreateAccountRequest>();
        WebApiContext.EnsureRequestInitialized(createAccountRequest);
        
        webApiContext.AccountsRepository.Received(0).CreateAccountAsync(Arg.Is<IAccount>(a => a.Login == createAccountRequest.UserName), Arg.Any<CancellationToken>());
    }

    #endregion
    
    #region Account Validation

    [When(@"I validate the account")]
    public async Task WhenIValidateTheAccount()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/account/validate");
        var authenticatedUser = scenarioContext.Get<AppUser>();
        if (authenticatedUser != null)
        {
            request.Headers.Add("Mock-Authenticated-UserId", authenticatedUser.AccountId.ToString());
        }

        scenarioContext.Set(await webApiContext.Client.SendAsync(request));
    }

    #endregion

    #region Account Sign In

    [Given(@"I have valid credentials")]
    public void GivenIHaveValidCredentials()
    {
        var userName = "testUserName";
        var password = "testPassword";
        var authenticateRequest = new AuthenticateRequest(userName, password);

        var accountDto = new AccountDto(Guid.NewGuid(), userName, new TrollDto(1, "testTrollName", "testScriptToken"));
        var signInAccount = new Domain.Accounts.Account(accountDto, password, new Encryptor());

        webApiContext.AccountsRepository.GetAccountByLoginAsync(userName, Arg.Any<CancellationToken>())
            .Returns(signInAccount);
        
        scenarioContext.Set(authenticateRequest);
        scenarioContext.Set(signInAccount);
    }

    [Given(@"I have invalid credentials")]
    public void GivenIHaveInvalidCredentials()
    {
        var userName = "testUserName";
        var password = "testPassword";
        var authenticateRequest = new AuthenticateRequest(userName, password);

        webApiContext.AccountsRepository.GetAccountByLoginAsync(userName, Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        scenarioContext.Set(authenticateRequest);
    }

    [When(@"I sign in")]
    public async Task WhenISignIn()
    {
        var authenticateRequest = scenarioContext.Get<AuthenticateRequest>();
        WebApiContext.EnsureRequestInitialized(authenticateRequest);
        
        scenarioContext.Set(await webApiContext.Client.PostAsJsonAsync("api/account/signin", authenticateRequest));
    }

    #endregion

    #region Account Sign Out

    [When(@"I sign out")]
    public async Task WhenISignOut()
    {
        var authenticatedUser = scenarioContext.Get<AppUser>();
        var request = new HttpRequestMessage(HttpMethod.Post, "api/account/signout");
        if (authenticatedUser != null)
        {
            request.Headers.Add("Mock-Authenticated-UserId", authenticatedUser.AccountId.ToString());
        }

        scenarioContext.Set(await webApiContext.Client.SendAsync(request));
    }

    #endregion
}
