using Refit;
using TrollIt.Infrastructure.Mountyhall;
using TrollIt.Infrastructure.Mountyhall.Errors.Abstractions;
using TrollIt.Infrastructure.Mountyhall.Handlers;
using TrollIt.Infrastructure.Mountyhall.Models;
using TrollIt.Infrastructure.Mountyhall.Readers;
using TrollIt.Infrastructure.Mountyhall.Readers.Abstractions;
using TrollIt.Infrastructure.Mountyhall.Readers.Common;
using TrollIt.Infrastructure.Mountyhall.Readers.Errors;

namespace Microsoft.Extensions.DependencyInjection;

internal static class MountyhallExtensions
{
    private const string PlainTextContentServiceKey = "PlainText";
    private const string JsonScriptsServiceKey = "JsonPublicScripts";

    public static void AddMountyhall(this IServiceCollection services, MountyhallOptions mountyhallOptions)
    {
        services.AddSingleton<IStreamReader, DefaultStreamReader>();
        services.AddKeyedSingleton<IHttpContentSerializer, PlainTextContentSerializer>(PlainTextContentServiceKey);
        services.AddKeyedSingleton<IHttpContentSerializer, TrollIt.Infrastructure.Mountyhall.Readers.Common.JsonContentSerializer>(JsonScriptsServiceKey);
        services.AddSingleton<IPublicScriptErrorProvider, PublicScriptErrorProvider>();

        services.AddTransient<ScriptHistoryHandler>();

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
                client.BaseAddress = new Uri(mountyhallOptions.Ftp);
            });

        services
            .AddRefitClient<IJsonScripts>(sp => new RefitSettings
            {
                ContentSerializer = sp.GetRequiredKeyedService<IHttpContentSerializer>(JsonScriptsServiceKey)
            })
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(mountyhallOptions.PublicScript);
            })
            .AddHttpMessageHandler<ScriptHistoryHandler>();
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
