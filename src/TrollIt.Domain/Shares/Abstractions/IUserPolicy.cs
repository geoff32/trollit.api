namespace TrollIt.Domain.Shares.Abstractions;

public interface IUserPolicy
{
    int TrollId { get; }
    IEnumerable<ITrollRight> Rights { get; }
    
    bool CanRead(FeatureId featureId, int trollId);
    bool CanRefresh(FeatureId featureId, int trollId);
}
