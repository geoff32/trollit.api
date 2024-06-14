using NpgsqlTypes;

namespace TrollIt.Infrastructure.Shares.Models;

internal enum FeatureStatus
{
    [PgName("inactive")]
    Inactive,
    [PgName("read")]
    Read,
    [PgName("readwrite")]
    Readwrite
}
