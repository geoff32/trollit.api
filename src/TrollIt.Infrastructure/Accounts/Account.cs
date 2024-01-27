using NpgsqlTypes;

namespace TrollIt.Infrastructure;

public record class Account
(
    [PgName("id")]
    Guid Id,
    [PgName("login")]
    string Login,
    [PgName("password")]
    byte[] Password
);
