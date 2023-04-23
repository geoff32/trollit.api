using Microsoft.Extensions.DependencyInjection;
using TrollIt.Application.Account.DependencyInjection;

namespace TrollIt.Application.DependencyInjection;

public static class ApplicationExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAccount();
    }
}