using TrollIt.Domain.Bestiaries.Acl;
using TrollIt.Domain.Bestiaries.Acl.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

internal static class BestiariesExtensions
{
    public static void AddBestiaries(this IServiceCollection services)
    {
        services.AddSingleton<IBestiariesAcl, BestiariesAcl>();
    }
}
