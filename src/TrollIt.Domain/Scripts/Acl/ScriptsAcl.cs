using TrollIt.Domain.Scripts.Abstractions;
using TrollIt.Domain.Scripts.Acl.Abstractions;
using TrollIt.Domain.Scripts.Acl.Models;

namespace TrollIt.Domain.Scripts.Acl;

internal class ScriptsAcl : IScriptsAcl
{
    public ITrollScript ToDomain(TrollScriptDto trollScriptDto)
        => new TrollScript(trollScriptDto);
}
