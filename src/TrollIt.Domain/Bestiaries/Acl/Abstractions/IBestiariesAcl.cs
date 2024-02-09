using TrollIt.Domain.Bestiaries.Abstractions;
using TrollIt.Domain.Bestiaries.Acl.Models;

namespace TrollIt.Domain.Bestiaries.Acl.Abstractions;

public interface IBestiariesAcl
{
    ITroll ToDomain(TrollDto trollDto);
}
