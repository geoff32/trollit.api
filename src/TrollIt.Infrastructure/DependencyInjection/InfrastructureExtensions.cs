using TrollIt.Domain.Accounts.Infrastructure.Abstractions;
using TrollIt.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class InfrastructureExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, InfrastructureOptions options)
    {
        services.AddSingleton<IPasswordEncryptor, Encryptor>();

        services.AddNpgsqlDataSource(options.ConnectionString);

        services.AddAccount();
    }
}
