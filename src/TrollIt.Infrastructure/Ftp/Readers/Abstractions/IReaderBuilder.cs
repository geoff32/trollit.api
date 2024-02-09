namespace TrollIt.Infrastructure.Ftp.Readers.Abstractions;

internal interface IReaderBuilder
{
    IReaderBuilder AddReader<T, TReader>() where TReader : class, IContentReader<T>, new();
}