namespace TrollIt.Application.Scripts.Abstractions;

public interface IScriptsService
{
    Task CleanAsync(CancellationToken stoppingToken);
    Task TraceAsync(int trollId, string scriptPath, CancellationToken cancellationToken);
}
