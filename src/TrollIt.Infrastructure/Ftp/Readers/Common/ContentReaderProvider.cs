using TrollIt.Infrastructure.Ftp.Readers.Abstractions;

namespace TrollIt.Infrastructure.Ftp.Readers.Common;

internal class ContentReaderProvider : IContentReaderProvider
{
    private readonly IDictionary<Type, IContentReader> _readers;

    public ContentReaderProvider(IDictionary<Type, IContentReader> readers)
        => _readers = readers;

    public IContentReader<T> GetReader<T>()
        => _readers.TryGetValue(typeof(T), out var reader)
            ? (IContentReader<T>)reader
            : throw new KeyNotFoundException("Unable to find reader");
}