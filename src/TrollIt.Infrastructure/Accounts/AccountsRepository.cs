using System.Data;
using Dapper;
using Npgsql;
using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Infrastructure;
using TrollIt.Infrastructure.Accounts.Acl.Abstractions;
using TrollIt.Infrastructure.Accounts.Models;

namespace TrollIt.Infrastructure.Accounts;

internal class AccountsRepository(NpgsqlDataSource dataSource, IAccountRepositoryAcl accountRepositoryAcl) : IAccountsRepository
{
    public async Task CreateAccount(IAccount account)
    {
        using var connection = dataSource.CreateConnection();
        await connection.OpenAsync();

        await connection.ExecuteAsync
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
            commandType: CommandType.StoredProcedure
        );

    }

    public async Task<IAccount?> GetAccount(Guid id)
    {
        using var connection = dataSource.CreateConnection();
        await connection.OpenAsync();

        var data = await connection.QuerySingleOrDefaultAsync<Account>(
            "SELECT * FROM app.get_account(@pId)",
            new { pid = id },
            commandType: CommandType.Text);

        return accountRepositoryAcl.ToDomain(data);
    }

    public async Task<IAccount?> GetAccountByLogin(string login)
    {
        using var connection = dataSource.CreateConnection();
        await connection.OpenAsync();

        var data = await connection.QuerySingleOrDefaultAsync<Account>(
            "SELECT * FROM app.get_account_bylogin(@pLogin)",
            new { plogin = login },
            commandType: CommandType.Text);

        return accountRepositoryAcl.ToDomain(data);
    }
}
