namespace TrollIt.Domain.Shares.Abstractions;

public interface IUserPolicy
{
    int TrollId { get; }
    IEnumerable<ITrollRight> Rights { get; }

    void EnsureReadAccess(FeatureId featureId, int trollId);
    void EnsureRefreshAccess(FeatureId featureId, int trollId);
}
