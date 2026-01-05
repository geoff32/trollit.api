using NpgsqlTypes;

namespace TrollIt.Infrastructure.Shares.Models;

internal record TrollShare
(
    [PgName("trollid")]
    int Trollid,
    [PgName("status")]
    PolicyStatus Status,
    [PgName("features")]
    TrollFeature[] Features
);
