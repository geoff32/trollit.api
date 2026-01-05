using TrollIt.Application.Shares;
using TrollIt.Application.Shares.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

internal static class SharesExtensions
{
    public static void AddShares(this IServiceCollection services)
    {
        services.AddSingleton<ISharesService, SharesService>();
    }
}
