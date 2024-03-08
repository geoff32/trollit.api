using TrollIt.Domain.Profiles.Abstractions;
using TrollIt.Domain.Profiles.Acl.Abstractions;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Domain.Profiles.Acl;

internal class ProfilesAcl : IProfilesAcl
{
    public IProfile ToDomain(ProfileDto profileDto) => new Profile(profileDto);
}
