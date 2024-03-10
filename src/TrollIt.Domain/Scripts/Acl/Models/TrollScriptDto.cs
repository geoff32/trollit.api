namespace TrollIt.Domain.Scripts.Acl.Models;

public record TrollScriptDto(int TrollId, IEnumerable<ScriptCounterDto> ScriptCounters);
