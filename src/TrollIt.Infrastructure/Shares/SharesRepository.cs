using System.Data;
using Dapper;
using Npgsql;
using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Abstractions;
using TrollIt.Domain.Shares.Infrastructure;
using TrollIt.Infrastructure.Npgsql;
using TrollIt.Infrastructure.Shares.Acl.Abstractions;
using TrollIt.Infrastructure.Shares.Models;

namespace TrollIt.Infrastructure.Shares;

internal class SharesRepository(NpgsqlDataSource dataSource, ISharesRepositoryAcl sharesRepositoryAcl, ISharesAcl sharesAcl)
    : ISharesRepository
{
    public async Task<ISharePolicy?> GetSharePolicyAsync(Guid sharePolicyId, CancellationToken cancellationToken)
    {
        using var connection = dataSource.CreateConnection();
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

    public async Task<IEnumerable<ISharePolicy>> GetTrollPoliciesAsync(int trollId, CancellationToken cancellationToken)
    {
        using var connection = dataSource.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        var data = await connection.QueryAsync<SharePolicy>
        (
            new CommandDefinition
            (
                "SELECT id, name, trolls FROM app.get_trollsharepolicies(@pTrollId)",
                new { ptrollid = trollId },
                commandType: CommandType.Text,
                cancellationToken: cancellationToken
            )
        );

        return sharesRepositoryAcl.ToDomain(data);
    }

    public async Task<IUserPolicy> GetUserPolicyAsync(int trollId, CancellationToken cancellationToken)
    {
        var data = await GetTrollPoliciesAsync(trollId, cancellationToken);

        return sharesAcl.ToDomain(trollId, data);
    }

    public async Task SaveAsync(ISharePolicy sharePolicy, CancellationToken cancellationToken)
    {
        using var connection = dataSource.CreateConnection();
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
