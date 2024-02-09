namespace TrollIt.Infrastructure.Ftp.Readers.Abstractions;

internal interface IContentReader<T> : IContentReader
{
    T Read(string content);
}

internal interface IContentReader
{
}