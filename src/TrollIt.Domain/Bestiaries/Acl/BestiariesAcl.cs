using TrollIt.Domain.Bestiaries.Abstractions;
using TrollIt.Domain.Bestiaries.Acl.Abstractions;
using TrollIt.Domain.Bestiaries.Acl.Models;

namespace TrollIt.Domain.Bestiaries.Acl;

internal class BestiariesAcl : IBestiariesAcl
{
    public ITroll ToDomain(TrollDto trollDto) => new Troll(trollDto);
}
