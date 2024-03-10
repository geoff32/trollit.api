using TrollIt.Domain.Profiles.Abstractions;
using TrollIt.Infrastructure.Mountyhall.Models.Profile;

namespace TrollIt.Infrastructure.Profiles.Acl.Abstractions;

internal interface IProfilesRepositoryAcl
{
    IProfile ToDomain(Profile profile);
    IProfile? ToDomain(Models.Troll? data);
}
