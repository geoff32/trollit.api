using Dapper;
using Npgsql;
using System.Data;
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
    public async Task<IPolicy?> GetPolicyAsync(Guid policyId, CancellationToken cancellationToken)
    {
        await using var connection = dataSource.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        var data = await connection.QuerySingleOrDefaultAsync<Policy>
        (
            new CommandDefinition
            (
                "SELECT id, name, trolls FROM app.get_policy(@pPolicyId)",
                new { ppolicyid = policyId },
                commandType: CommandType.Text,
                cancellationToken: cancellationToken
            )
        );

        return sharesRepositoryAcl.ToDomain(data);
    }
    
    public async Task<IEnumerable<IPolicy>> GetTrollPoliciesAsync(int trollId, CancellationToken cancellationToken)
    {
        var data = await InternalGetTrollPoliciesAsync(trollId, cancellationToken);
        return sharesRepositoryAcl.ToDomain(data);
    }

    public async Task<IEnumerable<Policy>> InternalGetTrollPoliciesAsync(int trollId, CancellationToken cancellationToken)
    {
        await using var connection = dataSource.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        return await connection.QueryAsync<Policy>
        (
            new CommandDefinition
            (
                "SELECT id, name, trolls FROM app.get_trollpolicies(@pTrollId)",
                new { ptrollid = trollId },
                commandType: CommandType.Text,
                cancellationToken: cancellationToken
            )
        );
    }

    public async Task SaveAsync(IPolicy policy, CancellationToken cancellationToken)
    {
        await using var connection = dataSource.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await connection.ExecuteAsync
        (
            new CommandDefinition
            (
                "app.update_policy",
                new
                {
                    ppolicy = new CustomTypeParameter<Policy>(sharesRepositoryAcl.ToDataModel(policy), "app.policy")
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            )
        );
    }
}
