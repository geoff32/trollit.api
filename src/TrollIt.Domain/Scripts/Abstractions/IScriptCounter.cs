namespace TrollIt.Domain.Scripts.Abstractions;

public interface IScriptCounter
{
    IScript Script { get; }
    int Call { get; }
    int MaxCall { get; }

    void EnsureAccess();
}
