using TrollIt.Domain.Accounts.Acl;
using TrollIt.Domain.Accounts.Acl.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

internal static class AccountsExtensions
{
    public static void AddAccount(this IServiceCollection services)
    {
        services.AddSingleton<IAccountsAcl, AccountsAcl>();
    }
}
