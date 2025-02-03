using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using TrollIt.Api.Tests.Mock;
using TrollIt.Domain.Accounts.Infrastructure;
using TrollIt.Domain.Bestiaries.Infrastructure;
using TrollIt.Domain.Profiles.Infrastructure;
using TrollIt.Domain.Scripts.Infrastructure;
using TrollIt.Domain.Shares.Infrastructure;

namespace TrollIt.Api.Tests.Context;

public class WebApiContext : IClassFixture<WebApplicationFactory<Program>>
{
    public WebApiContext(WebApplicationFactory<Program> factory)
    {
        ScriptsRepository = Substitute.For<IScriptRepository>();
        AuthenticatedUserRepository = new MockAuthenticatedUsersRepository();
        AccountsRepository = Substitute.For<IAccountsRepository>();
        ProfilesRepository = Substitute.For<IProfilesRepository>();
        TrollBestiary = Substitute.For<ITrollBestiary>();
        ShareRepository = Substitute.For<ISharesRepository>();
        Client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.Replace(ServiceDescriptor.Singleton(ScriptsRepository));
                services.Replace(ServiceDescriptor.Singleton(AccountsRepository));
                services.Replace(ServiceDescriptor.Singleton(ProfilesRepository));
                services.Replace(ServiceDescriptor.Singleton(TrollBestiary));
                services.Replace(ServiceDescriptor.Singleton(ShareRepository));
                services.AddMockCookieAuthentication(AuthenticatedUserRepository);
            });
        }).CreateClient();
    }

    public HttpClient Client { get; }
    public IScriptRepository ScriptsRepository { get; }
    public MockAuthenticatedUsersRepository AuthenticatedUserRepository { get; }
    public IAccountsRepository AccountsRepository { get; }
    public IProfilesRepository ProfilesRepository { get; }
    public ITrollBestiary TrollBestiary { get; }
    public ISharesRepository ShareRepository { get; }

    public static void EnsureRequestInitialized<T>([NotNull] T? request)
        where T : class
    {
        if (request == null)
        {
            throw new InvalidOperationException("Request not initialized");
        }
    }

    public static void EnsureResponseInitialized([NotNull] HttpResponseMessage? response)
    {
        if (response == null)
        {
            throw new InvalidOperationException("Response not initialized");
        }
    }
}