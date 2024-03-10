using System.Text;
using TrollIt.Infrastructure.Mountyhall.Errors.Abstractions;
using TrollIt.Infrastructure.Mountyhall.Readers.Abstractions;

namespace TrollIt.Infrastructure.Mountyhall.Readers.Common;

internal class DefaultStreamReader : IStreamReader
{
    private readonly IPublicScriptErrorProvider _publicScriptErrorProvider;
    private readonly IContentReaderProvider _readerProvider;
    public DefaultStreamReader(IContentReaderProvider readerProvider, IPublicScriptErrorProvider publicScriptErrorProvider)
    {
        _readerProvider = readerProvider;
        _publicScriptErrorProvider = publicScriptErrorProvider;

    }

    public async Task<T> ReadAsync<T>(Stream stream)
    {
        var content = await GetContentAsync(stream);
        _publicScriptErrorProvider.EnsureContent(content);

        var reader = _readerProvider.GetReader<T>();

        return reader.Read(content);
    }

    protected virtual Encoding DefaultEncoding => Encoding.GetEncoding("ISO-8859-1");

    protected async Task<string> GetContentAsync(Stream stream)
    {
        using var reader = new StreamReader(stream, DefaultEncoding);
        return await reader.ReadToEndAsync();
    }
}