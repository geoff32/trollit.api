using TrollIt.Domain.Scripts.Abstractions;
using TrollIt.Domain.Scripts.Acl.Models;

namespace TrollIt.Domain.Scripts;

internal record Script(ScriptId Id, IScriptCategory Category, string Path, string Name) : IScript
{
    public Script(ScriptDto scriptDto)
        : this(scriptDto.Id, new ScriptCategory(scriptDto.Category), scriptDto.Path, scriptDto.Name)
    {
    }

    public bool IsSameCategory(IScript script)
        => script.Category == Category;
}
