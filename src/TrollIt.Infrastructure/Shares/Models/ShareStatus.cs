using NpgsqlTypes;

namespace TrollIt.Infrastructure.Shares.Models;

internal enum ShareStatus
{
    [PgName("owner")]
    Owner,
    [PgName("admin")]
    Admin,
    [PgName("user")]
    User,
    [PgName("guest")]
    Guest
}
