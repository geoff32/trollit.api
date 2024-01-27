using System.Data;
using Dapper;
using Npgsql;
using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Infrastructure.Abstractions;

namespace TrollIt.Infrastructure.Accounts;

internal class AccountRepository(NpgsqlDataSource dataSource) : IAccountRepository
{
    public async Task CreateAccount(IAccount account)
    {
        using var connection = dataSource.CreateConnection();
        await connection.OpenAsync();

        await connection.ExecuteAsync
        (
            "app.add_user",
            new
            {
                pid = account.Id,
                plogin = account.Login,
                ppassword = account.Password.Value.ToArray(),
                ptrollid = account.Troll.Id,
                pname = account.Troll.Name,
                pscripttoken = account.Troll.ScriptToken
            },
            commandType: CommandType.StoredProcedure
        );

    }
}
