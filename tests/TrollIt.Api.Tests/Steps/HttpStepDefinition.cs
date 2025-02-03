using System.Net;
using FluentAssertions;
using Reqnroll;
using TrollIt.Api.Tests.Context;

namespace TrollIt.Api.Tests.Steps;

[Binding]
public class HttpStepDefinition(
    VerifyContext verifyContext,
    ScenarioContext scenarioContext)
{
    [Then(@"The response should be ok")]
    public Task ThenTheResponseShouldBeOk()
    {
        var response = scenarioContext.Get<HttpResponseMessage>();
        WebApiContext.EnsureResponseInitialized(response);
        
        return response.Should().BeOk(verifyContext.Settings);
    }

    [Then(@"The response should be no content")]
    public void ThenTheResponseShouldBeNoContent()
    {
        var response = scenarioContext.Get<HttpResponseMessage>();
        WebApiContext.EnsureResponseInitialized(response);
        
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Then(@"The response should be bad request")]
    public Task ThenTheResponseShouldBeBadRequest()
    {
        var response = scenarioContext.Get<HttpResponseMessage>();
        WebApiContext.EnsureResponseInitialized(response);
        
        return response.Should().BeBadRequest(verifyContext.Settings);
    }

    [Then(@"The response should be unauthorized")]
    public Task ThenTheResponseShouldBeUnauthorized()
    {
        var response = scenarioContext.Get<HttpResponseMessage>();
        WebApiContext.EnsureResponseInitialized(response);
        
        return response.Should().BeUnauthorized(verifyContext.Settings);
    }

    [Then(@"The response should be forbidden")]
    public Task ThenTheResponseShouldBeForbidden()
    {
        var response = scenarioContext.Get<HttpResponseMessage>();
        WebApiContext.EnsureResponseInitialized(response);
        
        return response.Should().BeForbidden(verifyContext.Settings);
    }

    [Then(@"The response should be not found")]
    public Task ThenTheResponseShouldBeNotFound()
    {
        var response = scenarioContext.Get<HttpResponseMessage>();
        WebApiContext.EnsureResponseInitialized(response);
        
        return response.Should().BeNotFound(verifyContext.Settings);
    }

    [Then(@"The authentication cookie should be set")]
    public void ThenTheAuthenticationCookieShouldBeSet()
    {
        var response = scenarioContext.Get<HttpResponseMessage>();
        WebApiContext.EnsureResponseInitialized(response);
        
        response.Headers.TryGetValues("Set-Cookie", out var cookies);
        cookies.Should().NotBeNull().And.ContainSingle().Which.Should().StartWith(".AspNetCore.Cookies=")
            .And.NotStartWith(".AspNetCore.Cookies=;");
    }

    [Then(@"The authentication cookie should be unset")]
    public void ThenTheAuthenticationCookieShouldBeUnset()
    {
        var response = scenarioContext.Get<HttpResponseMessage>();
        WebApiContext.EnsureResponseInitialized(response);
        
        response.Headers.TryGetValues("Set-Cookie", out var cookies);
        cookies.Should().NotBeNull().And.ContainSingle().Which.Should().StartWith(".AspNetCore.Cookies=;");
    }

    [Then(@"The authentication cookie should not be set")]
    public void ThenTheAuthenticationCookieShouldNotBeSet()
    {
        var response = scenarioContext.Get<HttpResponseMessage>();
        WebApiContext.EnsureResponseInitialized(response);

        response.Headers.TryGetValues("Set-Cookie", out var cookies);
        cookies.Should().BeNull();
    }
}