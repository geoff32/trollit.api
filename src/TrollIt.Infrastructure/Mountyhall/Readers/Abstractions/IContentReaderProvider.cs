namespace TrollIt.Infrastructure.Mountyhall.Readers.Abstractions;

internal interface IContentReaderProvider
{
    IContentReader<T> GetReader<T>();
}