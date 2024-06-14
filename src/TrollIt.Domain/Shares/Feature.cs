using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;

namespace TrollIt.Domain.Shares;
internal record Feature(FeatureId Id, bool CanRead, bool CanRefresh) : IFeature
{
    public Feature(FeatureDto featureDto) : this(featureDto.Id, featureDto.CanRead, featureDto.CanRefresh)
    {
    }
}