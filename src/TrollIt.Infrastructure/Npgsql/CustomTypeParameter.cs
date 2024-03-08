using System.Data;
using Npgsql;
using static Dapper.SqlMapper;

namespace TrollIt.Infrastructure.Npgsql;

public class CustomTypeParameter<T>(T value, string dataTypeName) : ICustomQueryParameter
{
    private readonly T _value = value;
    private readonly string _dataTypeName = dataTypeName;

    public void AddParameter(IDbCommand command, string name)
    {
        var parameter = new NpgsqlParameter
        {
            ParameterName = name,
            Value = _value,
            DataTypeName = _dataTypeName
        };
        command.Parameters.Add(parameter);
    }
}
