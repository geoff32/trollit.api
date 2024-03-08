using TrollIt.Infrastructure.Profiles.Models;

namespace Npgsql;

public static class NpgsqlExtensions
{
    internal static NpgsqlDataSourceBuilder MapProfiles(this NpgsqlDataSourceBuilder dataSourceBuilder)
    {
        dataSourceBuilder.MapComposite<Troll>("app.troll");
        dataSourceBuilder.MapComposite<Profile>("app.profile");
        dataSourceBuilder.MapComposite<TrollIt.Infrastructure.Profiles.Models.Attribute>("app.attribute");

        return dataSourceBuilder;
    }
}
