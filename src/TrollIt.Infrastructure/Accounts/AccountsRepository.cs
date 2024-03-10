using System.Data;
using Dapper;
using Npgsql;
using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Infrastructure;
using TrollIt.Infrastructure.Accounts.Acl.Abstractions;
using TrollIt.Infrastructure.Accounts.Models;

namespace TrollIt.Infrastructure.Accounts;

internal class AccountsRepository(NpgsqlDataSource dataSource, IAccountsRepositoryAcl accountRepositoryAcl) : IAccountsRepository
{
    public async Task CreateAccountAsync(IAccount account, CancellationToken cancellationToken)
    {
        using var connection = dataSource.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await connection.ExecuteAsync
        (
            new CommandDefinition
            (
                "app.add_account",
                new
                {
                    pid = account.Id,
                    plogin = account.Login,
                    ppassword = account.Password.Value.ToArray(),
                    ptrollid = account.Troll.Id,
                    ptrollname = account.Troll.Name,
                    pscripttoken = account.Troll.ScriptToken
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            )
        );
    }

    public async Task<IAccount?> GetAccountAsync(Guid id, CancellationToken cancellationToken)
    {
        using var connection = dataSource.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        var data = await connection.QuerySingleOrDefaultAsync<Account>
        (
            new CommandDefinition
            (
                "SELECT * FROM app.get_account(@pId)",
                new { pid = id },
                commandType: CommandType.Text,
                cancellationToken: cancellationToken
            )
        );

        return accountRepositoryAcl.ToDomain(data);
    }

    public async Task<IAccount?> GetAccountByLoginAsync(string login, CancellationToken cancellationToken)
    {
        using var connection = dataSource.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        var data = await connection.QuerySingleOrDefaultAsync<Account>
        (
            new CommandDefinition
            (
                "SELECT * FROM app.get_account_bylogin(@pLogin)",
                new { plogin = login },
                commandType: CommandType.Text,
                cancellationToken: cancellationToken
            )
        );

        return accountRepositoryAcl.ToDomain(data);
    }
}
