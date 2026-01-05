using NpgsqlTypes;

namespace TrollIt.Infrastructure.Shares.Models;

internal record TrollFeature
(
    [PgName("id")]
    FeatureId Id,
    [PgName("status")]
    FeatureStatus Status
);
