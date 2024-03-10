using TrollIt.Domain.Scripts.Infrastructure;
using TrollIt.Infrastructure.Scripts;
using TrollIt.Infrastructure.Scripts.Acl;
using TrollIt.Infrastructure.Scripts.Acl.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

internal static class ScriptsExtensions
{
    public static void AddScripts(this IServiceCollection services)
    {
        services.AddSingleton<IScriptsRepositoryAcl, ScriptsRepositoryAcl>();
        services.AddSingleton<IScriptRepository, ScriptsRepository>();
        
        services.AddHostedService<ScriptsHostedService>();
    }
}
