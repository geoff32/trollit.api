using TrollIt.Infrastructure.Ftp.Readers.Abstractions;

namespace TrollIt.Infrastructure.Ftp.Readers.Common;

internal class ReaderBuilder : IReaderBuilder
{
    private readonly IDictionary<Type, IContentReader> _readers;

    public ReaderBuilder()
    {
        _readers = new Dictionary<Type, IContentReader>();
    }

    public IReaderBuilder AddReader<T, TReader>()
        where TReader : class, IContentReader<T>, new()
    {
        _readers.Add(typeof(T), new TReader());

        return this;
    }

    public IContentReaderProvider BuildReaderProvider()
        => new ContentReaderProvider(_readers);
}