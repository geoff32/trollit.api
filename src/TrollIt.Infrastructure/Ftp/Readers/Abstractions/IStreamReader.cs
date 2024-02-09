namespace TrollIt.Infrastructure.Ftp.Readers.Abstractions;

internal interface IStreamReader
{
    Task<T> ReadAsync<T>(Stream stream);
}