using TrollIt.Domain.Bestiaries.Infrastructure.Abstractions;
using TrollIt.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

internal static class BestiariesExtensions
{
    public static void AddBestiaries(this IServiceCollection services)
    {
        services.AddSingleton<ITrollBestiary, TrollBestiary>();
    }
}
