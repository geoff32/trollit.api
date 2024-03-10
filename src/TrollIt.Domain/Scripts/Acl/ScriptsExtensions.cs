using TrollIt.Domain.Scripts.Acl;
using TrollIt.Domain.Scripts.Acl.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

internal static class ScriptsExtensions
{
    public static void AddScripts(this IServiceCollection services)
    {
        services.AddSingleton<IScriptsAcl, ScriptsAcl>();
    }
}
