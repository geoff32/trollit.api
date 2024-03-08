using TrollIt.Domain.Profiles.Acl;
using TrollIt.Domain.Profiles.Acl.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

internal static class ProfilesExtensions
{
    public static void AddProfiles(this IServiceCollection services)
    {
        services.AddSingleton<IProfilesAcl, ProfilesAcl>();
    }
}
