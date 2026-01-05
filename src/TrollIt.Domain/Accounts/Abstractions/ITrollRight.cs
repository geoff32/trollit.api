namespace TrollIt.Domain.Accounts.Abstractions;

public interface ITrollRight
{
    int TrollId { get; }
    IEnumerable<IFeature> Features { get; }
}
