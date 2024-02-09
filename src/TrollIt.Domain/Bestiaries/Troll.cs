using TrollIt.Domain.Bestiaries.Abstractions;
using TrollIt.Domain.Bestiaries.Acl.Models;

namespace TrollIt.Domain.Bestiaries;

internal record class Troll(int Id, string Name, IBreed Breed, IGuild? Guild) : ITroll
{
    public Troll(TrollDto trollDto)
        : this(trollDto.Id, trollDto.Name, new Breed(trollDto.Breed), trollDto.Guild == null ? null : new Guild(trollDto.Guild))
    {
    }
}
