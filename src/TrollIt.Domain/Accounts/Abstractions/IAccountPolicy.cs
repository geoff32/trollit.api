namespace TrollIt.Domain.Accounts.Abstractions;

public interface IAccountPolicy
{
    int TrollId { get; }
    IEnumerable<ITrollRight> Rights { get; }
    
    bool CanRead(FeatureId featureId, int trollId);
    bool CanRefresh(FeatureId featureId, int trollId);
}
