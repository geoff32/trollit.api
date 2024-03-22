using TrollIt.Infrastructure.Mountyhall.Readers.Abstractions;

namespace TrollIt.Infrastructure.Mountyhall.Readers.Common;

internal class ReaderBuilder : IReaderBuilder
{
    private readonly Dictionary<Type, IContentReader> _readers;

    public ReaderBuilder()
    {
        _readers = [];
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