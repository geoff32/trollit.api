using TrollIt.Domain.Accounts.Acl;
using TrollIt.Domain.Accounts.Acl.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

internal static class AccountExtensions
{
    public static void AddAccount(this IServiceCollection services)
    {
        services.AddSingleton<IAccountAcl, AccountAcl>();
    }
}
