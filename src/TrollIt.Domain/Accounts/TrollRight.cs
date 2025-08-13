using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;

namespace TrollIt.Domain.Accounts;

internal record TrollRight(int TrollId, IEnumerable<IFeature> Features) : ITrollRight
{
    public TrollRight(TrollRightDto trollRightDto)
        : this(trollRightDto.TrollId, trollRightDto.Features.Select(feature => new Feature(feature)))
    {
    }
}
