using System.Reflection;
using Refit;
using TrollIt.Infrastructure.Ftp.Readers.Abstractions;

namespace TrollIt.Infrastructure.Ftp.Models;

internal class PlainTextContentSerializer : IHttpContentSerializer
{
    private readonly IStreamReader _streamReader;

    public PlainTextContentSerializer(IStreamReader streamReader)
        => _streamReader = streamReader;

    public async Task<T?> FromHttpContentAsync<T>(HttpContent content, CancellationToken cancellationToken = default)
    {
        using var stream = await content.ReadAsStreamAsync(cancellationToken);

        return await _streamReader.ReadAsync<T>(stream)
            .ConfigureAwait(false);
    }

    public string? GetFieldNameForProperty(PropertyInfo propertyInfo)
    {
        throw new NotImplementedException();
    }

    public HttpContent ToHttpContent<T>(T item)
    {
        throw new NotImplementedException();
    }
}
