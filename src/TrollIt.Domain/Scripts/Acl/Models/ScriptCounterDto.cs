namespace TrollIt.Domain.Scripts.Acl.Models;

public record ScriptCounterDto(ScriptDto Script, int Call, int MaxCall);
