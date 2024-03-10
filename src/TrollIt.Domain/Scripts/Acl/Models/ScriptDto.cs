using TrollIt.Domain.Scripts.Abstractions;

namespace TrollIt.Domain.Scripts.Acl.Models;

public record ScriptDto(ScriptId Id, ScriptCategoryDto Category, string Path, string Name);
