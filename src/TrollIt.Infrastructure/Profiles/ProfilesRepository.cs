using System.Data;
using Dapper;
using Npgsql;
using TrollIt.Domain.Profiles.Abstractions;
using TrollIt.Domain.Profiles.Infrastructure;
using TrollIt.Infrastructure.Mountyhall;
using TrollIt.Infrastructure.Npgsql;
using TrollIt.Infrastructure.Profiles.Acl.Abstractions;

namespace TrollIt.Infrastructure.Profiles;

internal class ProfilesRepository
(
    NpgsqlDataSource dataSource,
    IJsonScripts jsonScripts,
    IProfilesRepositoryAcl profilesRepositoryAcl
) : IProfilesRepository
{
    public async Task<IProfile?> GetProfileAsync(int trollId, CancellationToken cancellationToken)
    {
        var connection = dataSource.CreateConnection();
        await connection.OpenAsync();

        var data = await connection.QuerySingleOrDefaultAsync<Models.Troll>(
            "SELECT * FROM app.get_troll(@pTrollId)",
            new { ptrollid = trollId },
            commandType: CommandType.Text);

        return profilesRepositoryAcl.ToDomain(data);
    }

    public async Task<IProfile> RefreshProfileAsync(int trollId, string token, CancellationToken cancellationToken)
    {
        var profile = await jsonScripts.ProfileAsync(trollId, token, cancellationToken);

        await RefreshTrollAsync(dataSource, new Models.Troll(profile.Troll, profile.Caracs), cancellationToken);

        return profilesRepositoryAcl.ToDomain(profile);
    }

    private static async Task RefreshTrollAsync(NpgsqlDataSource dataSource, Models.Troll troll, CancellationToken cancellationToken)
    {
        var connection = dataSource.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await connection.ExecuteAsync
        (
            new CommandDefinition(
                "app.refresh_troll",
                new
                {
                    ptroll = new CustomTypeParameter<Models.Troll>(troll, "app.troll")
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            )
        );
    }
}
