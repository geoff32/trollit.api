namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAccounts();
        services.AddProfiles();
        services.AddScripts();
        services.AddShares();
    }
}