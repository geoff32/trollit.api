using TrollIt.Infrastructure.Shares.Models;

namespace Npgsql;

internal static class NpgsqlSharesExtensions
{
    internal static NpgsqlDataSourceBuilder MapShares(this NpgsqlDataSourceBuilder dataSourceBuilder)
    {
        dataSourceBuilder.MapEnum<FeatureId>("app.featureid");
        dataSourceBuilder.MapEnum<FeatureStatus>("app.featurestatus");
        dataSourceBuilder.MapEnum<PolicyStatus>("app.policystatus");
        dataSourceBuilder.MapComposite<TrollFeature>("app.trollfeature");
        dataSourceBuilder.MapComposite<TrollShare>("app.trollshare");
        dataSourceBuilder.MapComposite<Policy>("app.policy");

        return dataSourceBuilder;
    }
}
