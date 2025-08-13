using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;

namespace TrollIt.Domain.Accounts;
internal record Feature(FeatureId Id, bool CanRead, bool CanRefresh) : IFeature
{
    public Feature(FeatureDto featureDto) : this(featureDto.Id, featureDto.CanRead, featureDto.CanRefresh)
    {
    }
}