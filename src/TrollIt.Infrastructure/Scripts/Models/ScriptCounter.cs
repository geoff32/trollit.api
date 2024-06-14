using NpgsqlTypes;

namespace TrollIt.Infrastructure.Scripts.Models;

internal record ScriptCounter
(
    [PgName("script")]
    ScriptInfo Script,
    [PgName("call")]
    int Call,
    [PgName("maxcall")]
    int? Maxcall
);