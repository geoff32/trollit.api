using TrollIt.Domain.Accounts.Infrastructure.Abstractions;
using TrollIt.Infrastructure.Accounts;

namespace Microsoft.Extensions.DependencyInjection;

internal static class AccountExtensions
{
    public static void AddAccount(this IServiceCollection services)
    {
        services.AddSingleton<IAccountRepository, AccountRepository>();
    }
}
