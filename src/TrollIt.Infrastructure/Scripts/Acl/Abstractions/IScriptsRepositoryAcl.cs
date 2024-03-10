using DomainAbstractions = TrollIt.Domain.Scripts.Abstractions;
using TrollIt.Infrastructure.Scripts.Models;

namespace TrollIt.Infrastructure.Scripts.Acl.Abstractions;

internal interface IScriptsRepositoryAcl
{
    ScriptId ToDataModel(DomainAbstractions.ScriptId scriptId);
    DomainAbstractions.ITrollScript? ToDomain(TrollScripts? data);
    DomainAbstractions.ITrollScript ToDefaultDomain(int trollId, IEnumerable<ScriptInfo>? data);
}
