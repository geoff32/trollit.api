using NpgsqlTypes;

namespace TrollIt.Infrastructure.Scripts.Models;

internal enum ScriptId
{
    [PgName("profile")]
    Profile,
    [PgName("effect")]
    Effect,
    [PgName("view")]
    View,
    [PgName("equipment")]
    Equipment,
    [PgName("flies")]
    Flies
}
