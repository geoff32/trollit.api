using TrollIt.Domain.Shares.Infrastructure;
using TrollIt.Infrastructure.Shares;
using TrollIt.Infrastructure.Shares.Abstractions;
using TrollIt.Infrastructure.Shares.Acl;
using TrollIt.Infrastructure.Shares.Acl.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

internal static class SharesExtensions
{
    public static void AddShares(this IServiceCollection services)
    {
        services.AddSingleton<ISharesRepositoryAcl, SharesRepositoryAcl>();
        services.AddSingleton<SharesRepository>();
        services.AddSingleton<ISharesRepository>(sp => sp.GetRequiredService<SharesRepository>());
        services.AddSingleton<IInternalSharesRepository>(sp => sp.GetRequiredService<SharesRepository>());
    }
}
