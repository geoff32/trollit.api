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

namespace TrollIt.Api.Tests.Account;

public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly IAccountsService _accountsService;
    private readonly HttpClient _client;

    public AccountControllerTests(WebApplicationFactory<Program> factory)
    {
        _accountsService = Substitute.For<IAccountsService>();
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(_accountsService);
            });
        }).CreateClient();
    }

    [Fact]
    public async Task CreateAccountAsync_ReturnsOkResultAndSetCookie_WhenAccountIsCreated()
    {
        // Arrange
        const string email = "test@test.com";
        const int trollId = 1;
        var createAccountRequest = new CreateAccountRequest(email, "testPassword", trollId, "token");
        var account = new AccountResponse(Guid.NewGuid(), email, new TrollResponse(trollId, "trollName"));
        _accountsService.CreateAccountAsync(createAccountRequest, Arg.Any<CancellationToken>()).Returns(account);

        // Act
        var response = await _client.PostAsJsonAsync("api/account", createAccountRequest);
        var result = await response.Content.ReadFromJsonAsync<AccountResponse>();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        result.Should().BeEquivalentTo(account);

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
}