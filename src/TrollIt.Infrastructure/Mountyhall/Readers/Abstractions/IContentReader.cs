namespace TrollIt.Infrastructure.Mountyhall.Readers.Abstractions;

internal interface IContentReader<out T> : IContentReader
{
    T Read(string content);
}

internal interface IContentReader
{
}