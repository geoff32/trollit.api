using Dapper;
using Npgsql;
using System.Data;
using System.Runtime.CompilerServices;
using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Infrastructure;
using TrollIt.Infrastructure.Npgsql;
using TrollIt.Infrastructure.Shares.Abstractions;
using TrollIt.Infrastructure.Shares.Acl.Abstractions;
using TrollIt.Infrastructure.Shares.Models;

namespace TrollIt.Infrastructure.Shares;

internal class SharesRepository(NpgsqlDataSource dataSource, ISharesRepositoryAcl sharesRepositoryAcl)
    : ISharesRepository, IInternalSharesRepository
{
    public async Task<ISharePolicy?> GetSharePolicyAsync(Guid sharePolicyId, CancellationToken cancellationToken)
    {
        await using var connection = dataSource.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        var data = await connection.QuerySingleOrDefaultAsync<SharePolicy>
        (
            new CommandDefinition
            (
                "SELECT id, name, trolls FROM app.get_sharepolicy(@pSharePolicyId)",
                new { psharepolicyid = sharePolicyId },
                commandType: CommandType.Text,
                cancellationToken: cancellationToken
            )
        );

        return sharesRepositoryAcl.ToDomain(data);
    }

    public async Task<IEnumerable<SharePolicy>> InternalGetTrollPoliciesAsync(int trollId, CancellationToken cancellationToken)
    {
        await using var connection = dataSource.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        return await connection.QueryAsync<SharePolicy>
                (
                    new CommandDefinition
                    (
                        "SELECT id, name, trolls FROM app.get_trollsharepolicies(@pTrollId)",
                        new { ptrollid = trollId },
                        commandType: CommandType.Text,
                        cancellationToken: cancellationToken
                    )
                );
    }

    public async Task SaveAsync(ISharePolicy sharePolicy, CancellationToken cancellationToken)
    {
        await using var connection = dataSource.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await connection.ExecuteAsync
        (
            new CommandDefinition
            (
                "app.update_sharepolicy",
                new
                {
                    psharepolicy = new CustomTypeParameter<SharePolicy>(sharesRepositoryAcl.ToDataModel(sharePolicy), "app.sharepolicy")
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            )
        );
    }
}
