using TrollIt.Domain.Scripts.Abstractions;
using TrollIt.Domain.Scripts.Acl.Models;
using TrollIt.Domain.Scripts.Exceptions;

namespace TrollIt.Domain.Scripts;

internal record ScriptCategory(string Name, int MaxCall) : IScriptCategory
{
    public ScriptCategory(ScriptCategoryDto scriptCategoryDto)
        : this(scriptCategoryDto.Name, scriptCategoryDto.MaxCall)
    {
    }

    public void EnsureAccess(IEnumerable<IScriptCounter> scriptCounters)
    {
        if (scriptCounters.Sum(counter => counter.Call) >= MaxCall)
        {
            throw new DomainException<ScriptsExceptions>(ScriptsExceptions.MaxCallScript);
        }
    }
}
