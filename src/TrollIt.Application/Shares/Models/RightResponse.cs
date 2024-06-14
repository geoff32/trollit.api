using TrollIt.Domain.Shares.Abstractions;

namespace TrollIt.Application.Shares.Models;

public record RightResponse(bool CanRead, bool CanRefresh)
{
    public RightResponse(IFeature feature) : this(feature.CanRead, feature.CanRefresh)
    {
    }
}
