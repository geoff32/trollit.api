using Microsoft.Extensions.DependencyInjection;
using TrollIt.Application.Account.Abstractions;

namespace TrollIt.Application.Account.DependencyInjection;

internal static class AccountExtensions
{
    public static void AddAccount(this IServiceCollection services)
    {
        services.AddSingleton<IAccountService, AccountService>();
    }
}