namespace TrollIt.Domain.Scripts.Abstractions;

public interface IScriptCategory
{
    string Name { get; }
    int MaxCall { get; }

    void EnsureAccess(IEnumerable<IScriptCounter> scriptCounters);
}