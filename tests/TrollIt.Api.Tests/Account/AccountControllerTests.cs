using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TrollIt.Application.Accounts.Abstractions;
using TrollIt.Application.Accounts.Models;
using NSubstitute;
using System.Net.Http.Json;
using NSubstitute.ExceptionExtensions;
using TrollIt.Application;
using AppAccountsExceptions = TrollIt.Application.Accounts.Exceptions;
using DomainAccountsExceptions = TrollIt.Domain.Accounts.Exceptions;
using TrollIt.Domain;
using System.Net;
using TrollIt.Api.Tests.Mock;
using NSubstitute.ReturnsExtensions;

namespace TrollIt.Api.Tests.Account;

public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly IAccountsService _accountsService;
    private readonly HttpClient _client;
    private readonly MockAuthenticatedUsersRepository _authenticatedUserRepository;

    public AccountControllerTests(WebApplicationFactory<Program> factory)
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
    public async Task CreateAccountAsync_ReturnsOkResultAndSetCookie_WhenAccountIsCreated()
    {
        // Arrange
        const string userName = "testUserName";
        const int trollId = 1;
        var createAccountRequest = new CreateAccountRequest(userName, "testPassword", trollId, "testToken");
        var account = new AccountResponse(Guid.NewGuid(), userName, new TrollResponse(trollId, "testName"));
        _accountsService.CreateAccountAsync(createAccountRequest, Arg.Any<CancellationToken>()).Returns(account);

        // Act
        var response = await _client.PostAsJsonAsync("api/account", createAccountRequest);

        // Assert
        await response.Should().BeOk(account);

        // Assert cookie
        response.Headers.TryGetValues("Set-Cookie", out var cookies);
        cookies.Should().NotBeNull().And.ContainSingle().Which.Should().StartWith(".AspNetCore.Cookies=");
    }

    [Theory]
    [InlineData(AppAccountsExceptions.AccountExceptions.AccountNotFound, "Compte inconnu")]
    [InlineData(AppAccountsExceptions.AccountExceptions.AccountAlreadyExists, "Compte déjà existant")]
    internal async Task CreateAccountAsync_ReturnsBadRequestResult_WhenAccountCreationThrowAppException(AppAccountsExceptions.AccountExceptions exception, string expectedDetail)
    {
        // Arrange
        var createAccountRequest = new CreateAccountRequest("test@test.com", "testPassword", 1, "token");
        _accountsService.CreateAccountAsync(createAccountRequest, Arg.Any<CancellationToken>())
            .Throws(new AppException<AppAccountsExceptions.AccountExceptions>(exception));

        // Act
        var response = await _client.PostAsJsonAsync("api/account", createAccountRequest);

        // Assert
        await response.Should().BeBadRequest(expectedDetail);

        // Assert cookie
        response.Headers.TryGetValues("Set-Cookie", out var cookies);
        cookies.Should().BeNull();
    }

    [Theory]
    [InlineData(DomainAccountsExceptions.AccountsException.InsecurePassword, "Le mot de passe doit faire plus de 5 caractères !")]
    internal async Task CreateAccountAsync_ReturnsBadRequestResult_WhenAccountCreationThrowDomainException(DomainAccountsExceptions.AccountsException exception, string expectedDetail)
    {
        // Arrange
        var createAccountRequest = new CreateAccountRequest("test@test.com", "testPassword", 1, "token");
        _accountsService.CreateAccountAsync(createAccountRequest, Arg.Any<CancellationToken>())
            .Throws(new DomainException<DomainAccountsExceptions.AccountsException>(exception));

        // Act
        var response = await _client.PostAsJsonAsync("api/account", createAccountRequest);

        // Assert
        await response.Should().BeBadRequest(expectedDetail);
    }

    [Fact]
    public async Task ValidateAsync_ReturnsOkResult_WhenAuthenticatedUser()
    {
        // Arrange
        var user = new AppUser(Guid.NewGuid(), 1, "testName");
        _authenticatedUserRepository.AddUser(user);

        var accountResponse = new AccountResponse(user.AccountId, "test@test.com", new TrollResponse(user.TrollId, user.TrollName));
        _accountsService.GetAccountAsync(user.AccountId, Arg.Any<CancellationToken>()).Returns(accountResponse);

        // Act
        var request = new HttpRequestMessage(HttpMethod.Post, "api/account/validate");
        request.Headers.Add("Mock-Authenticated-UserId", user.AccountId.ToString());

        var response = await _client.SendAsync(request);

        // Assert
        await response.Should().BeOk(accountResponse);
    }

    [Fact]
    public async Task ValidateAsync_ReturnsUnauthorizedResult_WhenNotAuthenticatedUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _authenticatedUserRepository.Clear();

        // Act
        var response = await _client.PostAsync("api/account/validate", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        await _accountsService.Received(0).GetAccountAsync(userId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SignInAsync_ReturnsOkResultAndSetCookie_WhenAuthenticateIsSuccessful()
    {
        // Arrange
        var userName = "testUserName";
        var authenticateRequest = new AuthenticateRequest(userName, "testPassword");
        var accountResponse = new AccountResponse(Guid.NewGuid(), userName, new TrollResponse(1, "testName"));
        _accountsService.AuthenticateAsync(authenticateRequest, Arg.Any<CancellationToken>()).Returns(accountResponse);

        // Act
        var response = await _client.PostAsJsonAsync("api/account/signin", authenticateRequest);

        // Assert
        await response.Should().BeOk(accountResponse);

        // Assert cookie
        response.Headers.TryGetValues("Set-Cookie", out var cookies);
        cookies.Should().NotBeNull().And.ContainSingle().Which.Should().StartWith(".AspNetCore.Cookies=");
    }

    [Fact]
    public async Task SignInAsync_ReturnsUnauthorizedResult_WhenAuthenticateFail()
    {
        // Arrange
        var authenticateRequest = new AuthenticateRequest("testUserName", "testPassword");
        _accountsService.AuthenticateAsync(authenticateRequest, Arg.Any<CancellationToken>()).ReturnsNull();

        // Act
        var response = await _client.PostAsJsonAsync("api/account/signin", authenticateRequest);

        // Assert
        await response.Should().BeUnauthorized("Identifiant ou mot de passe invalide");

        // Assert cookie
        response.Headers.TryGetValues("Set-Cookie", out var cookies);
        cookies.Should().BeNull();
    }

    [Fact]
    public async Task SignOutAsync_ReturnsNotContentResultAndResetCookie_WhenAuthenticatedUser()
    {
        // Arrange
        var user = new AppUser(Guid.NewGuid(), 1, "testName");
        _authenticatedUserRepository.AddUser(user);

        // Act
        var request = new HttpRequestMessage(HttpMethod.Post, "api/account/signout");
        request.Headers.Add("Mock-Authenticated-UserId", user.AccountId.ToString());

        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Assert cookie
        response.Headers.TryGetValues("Set-Cookie", out var cookies);
        cookies.Should().NotBeNull().And.ContainSingle().Which.Should().StartWith(".AspNetCore.Cookies=;");
    }

    [Fact]
    public async Task SignOutAsync_ReturnsUnauthorized_WhenNoAuthenticatedUser()
    {
        // Arrange
        _authenticatedUserRepository.Clear();

        // Act
        var response = await _client.PostAsync("api/account/signout", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        // Assert cookie
        response.Headers.TryGetValues("Set-Cookie", out var cookies);
        cookies.Should().BeNull();
    }
}