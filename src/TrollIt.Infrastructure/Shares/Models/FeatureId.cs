using NpgsqlTypes;

namespace TrollIt.Infrastructure.Shares.Models;

internal enum FeatureId
{
    [PgName("profile")]
    Profile,
    [PgName("view")]
    View
}
