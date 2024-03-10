using TrollIt.Domain.Scripts.Abstractions;
using TrollIt.Domain.Scripts.Acl.Models;
using TrollIt.Domain.Scripts.Exceptions;

namespace TrollIt.Domain.Scripts;

internal record TrollScript(int TrollId, IReadOnlyCollection<IScriptCounter> ScriptCounters) : ITrollScript
{
    public TrollScript(TrollScriptDto trollScriptDto)
        : this(trollScriptDto.TrollId, trollScriptDto.ScriptCounters.Select(script => new ScriptCounter(script)).ToArray())
    {
    }

    public void EnsureAccess(IScriptCounter scriptCounter)
    {
        scriptCounter.Script.Category.EnsureAccess(ScriptCounters.Where(counter => counter.Script.IsSameCategory(scriptCounter.Script)));
        scriptCounter.EnsureAccess();
    }

    public IScriptCounter GetScriptCounter(string scriptPath)
        => ScriptCounters.FirstOrDefault(counter => counter.Script.Path.Equals(scriptPath, StringComparison.InvariantCultureIgnoreCase))
            ?? throw new DomainException<ScriptsExceptions>(ScriptsExceptions.UnkownScript);
}
