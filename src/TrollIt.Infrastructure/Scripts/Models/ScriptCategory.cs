using NpgsqlTypes;

namespace TrollIt.Infrastructure.Scripts.Models;

internal record ScriptCategory
(
    [PgName("name")]
    string Name,
    [PgName("maxcall")]
    int Maxcall
);