using Refit;
using TrollIt.Infrastructure.Ftp;
using TrollIt.Infrastructure.Ftp.Errors;
using TrollIt.Infrastructure.Ftp.Models;
using TrollIt.Infrastructure.Ftp.Readers;
using TrollIt.Infrastructure.Ftp.Readers.Common;

namespace Microsoft.Extensions.DependencyInjection;

internal static class FtpExtensions
{
    public static void AddFtp(this IServiceCollection services)
    {
        var errorProvider = new PublicScriptErrorProvider();
        var readerBuilder = new ReaderBuilder();
        readerBuilder.AddReader<IEnumerable<Troll>, TrollStreamReader>();
        readerBuilder.AddReader<IEnumerable<Guild>, GuildStreamReader>();

        var ftpTextSettings = new RefitSettings
        {
            ContentSerializer = new PlainTextContentSerializer(new DefaultStreamReader(
                readerBuilder.BuildReaderProvider(),
                errorProvider))
        };

        services.AddRefitClient<IFtpClient>(ftpTextSettings)
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri("http://ftp.mountyhall.com/");
            });
    }
}
