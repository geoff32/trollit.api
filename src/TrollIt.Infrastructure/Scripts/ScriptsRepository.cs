using System.Data;
using Dapper;
using Npgsql;
using TrollIt.Domain.Scripts.Abstractions;
using TrollIt.Domain.Scripts.Infrastructure;
using TrollIt.Infrastructure.Npgsql;
using TrollIt.Infrastructure.Scripts.Acl.Abstractions;
using TrollIt.Infrastructure.Scripts.Models;

namespace TrollIt.Infrastructure.Scripts;

internal class ScriptsRepository(NpgsqlDataSource dataSource, IScriptsRepositoryAcl scriptsRepositoryAcl) : IScriptRepository
{
    public async Task CleanHistoryAsync(DateTimeOffset beforeDate)
    {
        using var connection = dataSource.CreateConnection();
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            "app.clean_scriptshistory",
            new { pbeforedate = beforeDate },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<ITrollScript?> GetTrollScriptAsync(int trollId, CancellationToken cancellationToken)
    {
        using var connection = dataSource.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        var data = await connection.QuerySingleOrDefaultAsync<TrollScripts>
        (
            new CommandDefinition
            (
                "SELECT * FROM app.get_trollscripts(@pTrollId, @pFromDate)",
                new { ptrollid = trollId, pFromDate = DateTimeOffset.UtcNow.AddDays(-1) },
                commandType: CommandType.Text,
                cancellationToken: cancellationToken
            )
        );

        return scriptsRepositoryAcl.ToDomain(data);
    }

    public async Task<ITrollScript> InitializeTrollScriptAsync(int trollId, CancellationToken cancellationToken)
    {
        using var connection = dataSource.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        var data = await connection.QueryAsync<ScriptInfo>
        (
            new CommandDefinition
            (
                "SELECT * FROM app.get_scriptinfos()",
                commandType: CommandType.Text,
                cancellationToken: cancellationToken
            )
        );

        return scriptsRepositoryAcl.ToDefaultDomain(trollId, data);
    }

    public async Task TraceAsync(ITrollScript trollScript, Domain.Scripts.Abstractions.ScriptId scriptId, DateTimeOffset dateTime, CancellationToken cancellationToken)
    {
        using var connection = dataSource.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await connection.ExecuteAsync
        (
            new CommandDefinition
            (
                "app.add_scriptshistory",
                new
                {
                    ptrollid = trollScript.TrollId,
                    pscriptid = new CustomTypeParameter<Models.ScriptId>(scriptsRepositoryAcl.ToDataModel(scriptId), "app.scriptid"),
                    pdate = dateTime
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            )
        );
    }
}
