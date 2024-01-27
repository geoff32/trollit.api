using TrollIt.Application.Account.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAccount();
    }
}