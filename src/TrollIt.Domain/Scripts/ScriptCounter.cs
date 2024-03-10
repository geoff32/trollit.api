using TrollIt.Domain.Scripts.Abstractions;
using TrollIt.Domain.Scripts.Acl.Models;
using TrollIt.Domain.Scripts.Exceptions;

namespace TrollIt.Domain.Scripts;

internal record ScriptCounter(IScript Script, int Call, int MaxCall) : IScriptCounter
{
    public ScriptCounter(ScriptCounterDto scriptCounterDto)
        : this(new Script(scriptCounterDto.Script), scriptCounterDto.Call, scriptCounterDto.MaxCall)
    {
    }

    public void EnsureAccess()
    {
        if (Call >= MaxCall)
        {
            throw new DomainException<ScriptsExceptions>(ScriptsExceptions.MaxCallScript);
        }
    }
}
