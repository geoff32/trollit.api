using TrollIt.Domain.Profiles.Infrastructure;
using TrollIt.Infrastructure.Profiles;
using TrollIt.Infrastructure.Profiles.Acl;
using TrollIt.Infrastructure.Profiles.Acl.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

internal static class ProfilesExtensions
{
    public static void AddProfiles(this IServiceCollection services)
    {
        services.AddSingleton<IProfilesRepositoryAcl, ProfilesRepositoryAcl>();
        services.AddSingleton<IProfilesRepository, ProfilesRepository>();
    }
}
