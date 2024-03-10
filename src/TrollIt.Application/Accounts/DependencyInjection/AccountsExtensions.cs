using TrollIt.Application.Accounts;
using TrollIt.Application.Accounts.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

internal static class AccountsExtensions
{
    public static void AddAccounts(this IServiceCollection services)
    {
        services.AddSingleton<IAccountsService, AccountsService>();
    }
}