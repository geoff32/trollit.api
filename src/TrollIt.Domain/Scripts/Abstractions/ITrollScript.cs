namespace TrollIt.Domain.Scripts.Abstractions;

public interface ITrollScript
{
    int TrollId { get; }
    IReadOnlyCollection<IScriptCounter> ScriptCounters { get; }

    void EnsureAccess(IScriptCounter scriptCounter);
    IScriptCounter GetScriptCounter(string scriptPath);
}
