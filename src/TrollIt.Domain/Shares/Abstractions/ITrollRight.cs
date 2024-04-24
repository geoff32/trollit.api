namespace TrollIt.Domain.Shares.Abstractions;

public interface ITrollRight
{
    int TrollId { get; }
    IEnumerable<IFeature> Features { get; }
}
