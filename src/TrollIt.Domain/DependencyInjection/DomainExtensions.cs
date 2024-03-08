namespace Microsoft.Extensions.DependencyInjection;

public static class DomainExtensions
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddAccount();
        services.AddBestiaries();
        services.AddProfiles();
    }
}
