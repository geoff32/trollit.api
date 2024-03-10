using TrollIt.Domain.Scripts.Abstractions;

namespace TrollIt.Domain.Scripts.Infrastructure;

public interface IScriptRepository
{
    Task CleanHistoryAsync(DateTimeOffset beforeDate);
    Task<ITrollScript?> GetTrollScriptAsync(int trollId, CancellationToken cancellationToken);
    Task<ITrollScript> InitializeTrollScriptAsync(int trollId, CancellationToken cancellationToken);
    Task TraceAsync(ITrollScript trollScript, ScriptId scriptId, DateTimeOffset dateTime, CancellationToken cancellationToken);
}
