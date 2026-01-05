using NpgsqlTypes;

namespace TrollIt.Infrastructure.Scripts.Models;

internal record TrollScripts
(
    [PgName("trollid")]
    int Trollid,
    [PgName("trollname")]
    string Trollname,
    [PgName("profile")]
    ScriptCounter Profile,
    [PgName("effect")]
    ScriptCounter Effect,
    [PgName("view")]
    ScriptCounter View,
    [PgName("equipment")]
    ScriptCounter Equipment,
    [PgName("flies")]
    ScriptCounter Flies
);