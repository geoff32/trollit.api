namespace TrollIt.Infrastructure.Mountyhall.Readers.Abstractions;

internal interface IContentReader<T> : IContentReader
{
    T Read(string content);
}

internal interface IContentReader
{
}