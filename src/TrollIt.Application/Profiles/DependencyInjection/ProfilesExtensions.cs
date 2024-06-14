using TrollIt.Application.Profiles;
using TrollIt.Application.Profiles.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

internal static class ProfilesExtensions
{
    public static void AddProfiles(this IServiceCollection services)
    {
        services.AddSingleton<IProfilesService, ProfilesService>();
    }
}
