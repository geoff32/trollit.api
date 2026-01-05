using Argon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using TrollIt.Api.Tests.Mock;
using TrollIt.Application;
using TrollIt.Application.Accounts.Abstractions;

namespace TrollIt.Api.Tests.Exceptions;

public class UncaughtExceptionsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly IAccountsService _accountsService;
    private readonly MockAuthenticatedUsersRepository _authenticatedUserRepository;
    private readonly HttpClient _client;

    public UncaughtExceptionsTests(WebApplicationFactory<Program> factory)
    {
        _accountsService = Substitute.For<IAccountsService>();
        _authenticatedUserRepository = new MockAuthenticatedUsersRepository();
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(_accountsService);
                services.AddMockCookieAuthentication(_authenticatedUserRepository);
            });
        }).CreateClient();
    }

    [Fact]
    public async Task Action_ReturnsInternalServerErrorResult_WhenExceptionIsThrown()
    {
        // Arrange
        var user = new AppUser(Guid.NewGuid(), 1, "testName");
        _authenticatedUserRepository.AddUser(user);

        _accountsService.GetAccountAsync(user.AccountId, Arg.Any<CancellationToken>())
            .Throws(new Exception("Unhandled exception"));

        // Act
        var request = new HttpRequestMessage(HttpMethod.Post, "api/account/validate");
        request.Headers.Add("Mock-Authenticated-UserId", user.AccountId.ToString());

        var response = await _client.SendAsync(request);

        // Assert
        var verifySettings = new VerifySettings();
        verifySettings.AddExtraSettings(settings =>
        {
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
        });
        
        verifySettings.UseDirectory("Exceptions");
        verifySettings.UseFileName("Unhandled exception throw 500");
        await response
            .Should().NotBeNull()
            .And.Be500InternalServerError()
            .And.VerifyContentAsync<ProblemDetails>(verifySettings);
    }
}
