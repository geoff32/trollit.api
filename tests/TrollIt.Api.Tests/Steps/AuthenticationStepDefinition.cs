using System.Net;
using FluentAssertions;
using Reqnroll;
using TrollIt.Api.Tests.Context;
using TrollIt.Application;

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

    [Given(@"I am an unauthenticated user")]
    public void GivenIAmAnUnauthenticatedUser()
    {
        webApiContext.AuthenticatedUserRepository.Clear();
        scenarioContext.Set((AppUser?)null);
    }
}