using TrollIt.Domain.Bestiaries.Abstractions;
using TrollIt.Domain.Bestiaries.Acl.Models;

namespace TrollIt.Domain.Bestiaries;

internal record class Guild(int Id, string Name) : IGuild
{
    public Guild(GuildDto guild)
        : this(guild.Id, guild.Name)
    {
    }
}
