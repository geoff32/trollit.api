using Npgsql;
using TrollIt.Domain.Accounts.Infrastructure;
using TrollIt.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class InfrastructureExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, InfrastructureOptions options)
    {
        services.AddSingleton<IPasswordEncryptor, Encryptor>();

        services.AddNpgsqlDataSource(options.ConnectionString, builder =>
        {
            builder.MapProfiles();
            builder.MapScripts();
        });

        services.AddMemoryCache();

        services.AddMountyhall(options.Mountyhall);

        services.AddAccounts();
        services.AddBestiaries();
        services.AddProfiles();
        services.AddScripts();
    }
}
