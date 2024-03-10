using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrollIt.Application.Scripts.Abstractions;

namespace TrollIt.Infrastructure.Scripts;

public class ScriptsHostedService(IScriptsService scriptsService, ILogger<ScriptsHostedService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            try
            {
                await scriptsService.CleanAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Clean script history failed");
            }
            finally
            {
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
