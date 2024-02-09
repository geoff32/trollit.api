using TrollIt.Domain.Accounts.Infrastructure.Abstractions;
using TrollIt.Infrastructure.Accounts;
using TrollIt.Infrastructure.Accounts.Acl;
using TrollIt.Infrastructure.Accounts.Acl.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

internal static class AccountsExtensions
{
    public static void AddAccounts(this IServiceCollection services)
    {
        services.AddSingleton<IAccountRepositoryAcl, AccountRepositoryAcl>();
        services.AddSingleton<IAccountRepository, AccountRepository>();
    }
}
