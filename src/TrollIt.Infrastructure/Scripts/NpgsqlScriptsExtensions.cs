using TrollIt.Infrastructure.Scripts.Models;

namespace Npgsql;

internal static class NpgsqlScriptsExtensions
{
    internal static NpgsqlDataSourceBuilder MapScripts(this NpgsqlDataSourceBuilder dataSourceBuilder)
    {
        dataSourceBuilder.MapEnum<ScriptId>("app.scriptid");
        dataSourceBuilder.MapComposite<TrollScripts>("app.trollscripts");
        dataSourceBuilder.MapComposite<ScriptCounter>("app.scriptcounter");
        dataSourceBuilder.MapComposite<ScriptInfo>("app.scriptinfo");
        dataSourceBuilder.MapComposite<ScriptCategory>("app.scriptcategory");

        return dataSourceBuilder;
    }
}
