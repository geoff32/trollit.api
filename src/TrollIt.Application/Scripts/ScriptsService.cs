using TrollIt.Application.Scripts.Abstractions;
using TrollIt.Domain.Scripts.Infrastructure;

namespace TrollIt.Application.Scripts;

internal class ScriptsService(IScriptRepository scriptRepository) : IScriptsService
{
    public Task CleanAsync(CancellationToken stoppingToken) =>
        scriptRepository.CleanHistoryAsync(DateTimeOffset.UtcNow.AddDays(-2), stoppingToken);

    public async Task TraceAsync(int trollId, string scriptPath, CancellationToken cancellationToken)
    {
        var script = await scriptRepository.GetTrollScriptAsync(trollId, cancellationToken)
            ?? await scriptRepository.InitializeTrollScriptAsync(trollId, cancellationToken);

        var scriptCounter = script.GetScriptCounter(scriptPath);
        script.EnsureAccess(scriptCounter);

        await scriptRepository.TraceAsync(script, scriptCounter.Script.Id, DateTimeOffset.UtcNow, cancellationToken);
    }
}
