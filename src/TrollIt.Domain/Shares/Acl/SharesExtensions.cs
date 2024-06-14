using TrollIt.Domain.Shares.Acl;
using TrollIt.Domain.Shares.Acl.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

internal static class SharesExtensions
{
    public static void AddShares(this IServiceCollection services)
    {
        services.AddSingleton<ISharesAcl, SharesAcl>();
    }
}
