using System.Reflection;
using System.Text.Json;
using Refit;
using TrollIt.Infrastructure.Mountyhall.Errors.Abstractions;

namespace TrollIt.Infrastructure.Mountyhall.Readers.Common;

internal class JsonContentSerializer : IHttpContentSerializer
{
    private readonly IPublicScriptErrorProvider _publicScriptErrorProvider;
    private readonly JsonSerializerOptions _serializerOptions;

    public JsonContentSerializer(IPublicScriptErrorProvider publicScriptErrorProvider, JsonSerializerOptions? serializerOptions = null)
    {
        _publicScriptErrorProvider = publicScriptErrorProvider;
        _serializerOptions = serializerOptions ?? new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };
    }

    public async Task<T?> FromHttpContentAsync<T>(HttpContent content, CancellationToken cancellationToken = default)
    {
        var contentString = await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        _publicScriptErrorProvider.EnsureContent(contentString);

        return contentString == null ? default : JsonSerializer.Deserialize<T?>(contentString, _serializerOptions);
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