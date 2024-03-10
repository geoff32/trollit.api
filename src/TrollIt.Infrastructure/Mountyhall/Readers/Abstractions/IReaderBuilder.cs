namespace TrollIt.Infrastructure.Mountyhall.Readers.Abstractions;

internal interface IReaderBuilder
{
    IReaderBuilder AddReader<T, TReader>() where TReader : class, IContentReader<T>, new();
    IContentReaderProvider BuildReaderProvider();
}