namespace TrollIt.Infrastructure.Mountyhall.Readers.Abstractions;

internal interface IStreamReader
{
    Task<T> ReadAsync<T>(Stream stream);
}