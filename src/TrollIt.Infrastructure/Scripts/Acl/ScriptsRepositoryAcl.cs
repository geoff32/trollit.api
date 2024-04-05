using DomainAbstractions = TrollIt.Domain.Scripts.Abstractions;
using TrollIt.Infrastructure.Scripts.Acl.Abstractions;
using TrollIt.Infrastructure.Scripts.Models;
using TrollIt.Domain.Scripts.Acl.Abstractions;
using TrollIt.Domain.Scripts.Acl.Models;

namespace TrollIt.Infrastructure.Scripts.Acl;

internal class ScriptsRepositoryAcl(IScriptsAcl scriptsAcl) : IScriptsRepositoryAcl
{
    public ScriptId ToDataModel(DomainAbstractions.ScriptId scriptId) => scriptId switch
    {
        DomainAbstractions.ScriptId.Profile => ScriptId.Profile,
        DomainAbstractions.ScriptId.Effect => ScriptId.Effect,
        DomainAbstractions.ScriptId.View => ScriptId.View,
        DomainAbstractions.ScriptId.Equipment => ScriptId.Equipment,
        DomainAbstractions.ScriptId.Flies => ScriptId.Flies,
        _ => throw new ArgumentOutOfRangeException(nameof(scriptId))
    };

    public DomainAbstractions.ITrollScript? ToDomain(TrollScripts? data) =>
        data == null ? null : scriptsAcl.ToDomain(new TrollScriptDto
        (
            data.TrollId,
            new[] { data.Profile, data.Effect, data.View, data.Equipment, data.Flies }
                .Select(ToDomain)
        ));

    public DomainAbstractions.ITrollScript ToDefaultDomain(int trollId, IEnumerable<ScriptInfo>? data) =>
        scriptsAcl.ToDomain(new TrollScriptDto
        (
            trollId,
            data?.Select(ToDefaultDomain) ?? []
        ));

    private ScriptCounterDto ToDefaultDomain(ScriptInfo data)
    {
        return new ScriptCounterDto
        (
            Script: new ScriptDto
            (
                Id: ToDomain(data.Id),
                Category: new ScriptCategoryDto(data.Category.Name, data.Category.Maxcall),
                Path: data.Script,
                Name: data.Name
            ),
            Call: 0,
            MaxCall: data.Category.Maxcall
        );
    }

    private ScriptCounterDto ToDomain(ScriptCounter data)
    {
        return new ScriptCounterDto
        (
            Script: new ScriptDto
            (
                Id: ToDomain(data.Script.Id),
                Category: new ScriptCategoryDto(data.Script.Category.Name, data.Script.Category.Maxcall),
                Path: data.Script.Script,
                Name: data.Script.Name
            ),
            Call: data.Call,
            MaxCall: data.Maxcall
        );
    }

    private static DomainAbstractions.ScriptId ToDomain(ScriptId scriptId) => scriptId switch
    {
        ScriptId.Profile => DomainAbstractions.ScriptId.Profile,
        ScriptId.Effect => DomainAbstractions.ScriptId.Effect,
        ScriptId.View => DomainAbstractions.ScriptId.View,
        ScriptId.Equipment => DomainAbstractions.ScriptId.Equipment,
        ScriptId.Flies => DomainAbstractions.ScriptId.Flies,
        _ => throw new ArgumentOutOfRangeException(nameof(scriptId))
    };
}
