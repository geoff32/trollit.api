using TrollIt.Domain.Scripts.Abstractions;
using TrollIt.Domain.Scripts.Acl.Models;

namespace TrollIt.Domain.Scripts.Acl.Abstractions;

public interface IScriptsAcl
{
    ITrollScript ToDomain(TrollScriptDto trollScriptDto);
}
