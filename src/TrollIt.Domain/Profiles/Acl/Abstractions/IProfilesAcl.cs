using TrollIt.Domain.Profiles.Abstractions;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Domain.Profiles.Acl.Abstractions;

public interface IProfilesAcl
{
    IProfile ToDomain(ProfileDto profileDto);
}
