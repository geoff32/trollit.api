namespace TrollIt.Infrastructure.Ftp.Readers.Abstractions;

internal interface IContentReaderProvider
{
    IContentReader<T> GetReader<T>();
}