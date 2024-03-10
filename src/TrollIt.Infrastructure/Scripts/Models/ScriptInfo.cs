using NpgsqlTypes;

namespace TrollIt.Infrastructure.Scripts.Models;

internal record ScriptInfo
(
    [PgName("id")]
    ScriptId Id,
    [PgName("name")]
    string Name,
    [PgName("script")]
    string Script,
    [PgName("category")]
    ScriptCategory Category
);