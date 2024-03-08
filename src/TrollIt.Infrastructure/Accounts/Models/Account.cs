using NpgsqlTypes;

namespace TrollIt.Infrastructure.Accounts.Models;

internal record Account
(
    [PgName("id")]
    Guid Id,
    [PgName("login")]
    string Login,
    [PgName("password")]
    byte[] Password,
    [PgName("trollid")]
    int TrollId,
    [PgName("trollname")]
    string TrollName,
    [PgName("scripttoken")]
    string ScriptToken
);
