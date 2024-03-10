namespace TrollIt.Domain.Scripts.Abstractions;

public interface IScript
{
    ScriptId Id { get; }
    IScriptCategory Category { get; }
    string Path { get; }
    string Name { get; }

    bool IsSameCategory(IScript script);
}
