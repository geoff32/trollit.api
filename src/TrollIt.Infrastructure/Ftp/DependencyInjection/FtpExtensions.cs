using Refit;
using TrollIt.Infrastructure.Ftp;
using TrollIt.Infrastructure.Ftp.Errors.Abstractions;
using TrollIt.Infrastructure.Ftp.Models;
using TrollIt.Infrastructure.Ftp.Readers;
using TrollIt.Infrastructure.Ftp.Readers.Abstractions;
using TrollIt.Infrastructure.Ftp.Readers.Common;
using TrollIt.Infrastructure.Ftp.Readers.Errors;

namespace Microsoft.Extensions.DependencyInjection;

internal static class FtpExtensions
{
    private const string PlainTextContentServiceKey = "PlainText";
    public static void AddFtp(this IServiceCollection services)
    {
        services.AddSingleton<IStreamReader, DefaultStreamReader>();
        services.AddKeyedSingleton<IHttpContentSerializer, PlainTextContentSerializer>(PlainTextContentServiceKey);
        services.AddSingleton<IPublicScriptErrorProvider, PublicScriptErrorProvider>();

        var errorProvider = new PublicScriptErrorProvider();
        services.AddReader(readerBuilder =>
        {
            readerBuilder.AddReader<IEnumerable<Troll>, TrollStreamReader>();
            readerBuilder.AddReader<IEnumerable<Guild>, GuildStreamReader>();
        });

        services.AddRefitClient<IFtpClient>(sp =>new RefitSettings
            {
                ContentSerializer = sp.GetRequiredKeyedService<IHttpContentSerializer>(PlainTextContentServiceKey)
            })
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri("http://ftp.mountyhall.com/");
            });
    }
    public static void AddReader(this IServiceCollection services, Action<IReaderBuilder> configure)
    {
        services.AddSingleton<IReaderBuilder, ReaderBuilder>();
        services.AddSingleton(sp =>
        {
            var builder = sp.GetRequiredService<IReaderBuilder>();
            configure(builder);
            return builder.BuildReaderProvider();
        });
    }
}
