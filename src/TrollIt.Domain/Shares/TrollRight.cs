using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;

namespace TrollIt.Domain.Shares;

internal record TrollRight(int TrollId, IEnumerable<IFeature> Features) : ITrollRight
{
    public TrollRight(TrollRightDto trollRightDto)
        : this(trollRightDto.TrollId, trollRightDto.Features.Select(feature => new Feature(feature)))
    {
    }
}
