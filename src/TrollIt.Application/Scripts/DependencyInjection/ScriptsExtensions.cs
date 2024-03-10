using TrollIt.Application.Scripts;
using TrollIt.Application.Scripts.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

internal static class ScriptsExtensions
{
    public static void AddScripts(this IServiceCollection services)
    {
        services.AddSingleton<IScriptsService, ScriptsService>();
    }
}
