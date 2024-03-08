using Npgsql;
using Refit;
using TrollIt.Domain.Profiles.Infrastructure;
using TrollIt.Infrastructure.Profiles;
using TrollIt.Infrastructure.Profiles.Scripts;
using Common = TrollIt.Infrastructure.Ftp.Readers.Common;

namespace Microsoft.Extensions.DependencyInjection;

internal static class ProfilesExtensions
{
    private const string JsonScriptsServiceKey = "JsonPublicScripts";

    public static void AddProfiles(this IServiceCollection services)
    {
        services.AddSingleton<IProfilesRepository, ProfilesRepository>();
        services.AddKeyedSingleton<IHttpContentSerializer, Common.JsonContentSerializer>(JsonScriptsServiceKey);

        services.AddRefitClient<IJsonScripts>(sp => new RefitSettings
        {
            ContentSerializer = sp.GetRequiredKeyedService<IHttpContentSerializer>(JsonScriptsServiceKey)
        })
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri("http://sp.mountyhall.com/");
            });
    }
}
