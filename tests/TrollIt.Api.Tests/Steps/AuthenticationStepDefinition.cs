using System.Net;
using FluentAssertions;
using NSubstitute;
using Reqnroll;
using TrollIt.Api.Tests.Context;
using TrollIt.Application;
using TrollIt.Domain.Accounts;
using TrollIt.Domain.Accounts.Abstractions;

namespace TrollIt.Api.Tests.Steps;

[Binding]
public class AuthenticationStepDefinition(
    WebApiContext webApiContext,
    ScenarioContext scenarioContext)
{
    [Given(@"I am an authenticated user")]
    public void GivenIAmAnAuthenticatedUser()
    {
        var user = new AppUser(Guid.NewGuid(), 1, "testName");
        webApiContext.AuthenticatedUserRepository.AddUser(user);
        scenarioContext.Set(user);
    }
    
    [Given(@"I have an existing account")]
    public void GivenIHaveAnExistingAccount()
    {
        var user = scenarioContext.Get<AppUser>();
        webApiContext.AccountsRepository.GetAccountAsync(user.AccountId, Arg.Any<CancellationToken>())
            .Returns(new Domain.Accounts.Account(user.AccountId, "Login", Substitute.For<IPassword>(),
                new Troll(user.TrollId, user.TrollName, "ScriptToken")));
    }

    [Given(@"I am an unauthenticated user")]
    public void GivenIAmAnUnauthenticatedUser()
    {
        webApiContext.AuthenticatedUserRepository.Clear();
        scenarioContext.Set((AppUser?)null);
    }
}