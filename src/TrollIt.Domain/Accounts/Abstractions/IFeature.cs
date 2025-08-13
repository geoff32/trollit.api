namespace TrollIt.Domain.Accounts.Abstractions;

public interface IFeature
{
    FeatureId Id { get; }

    bool CanRead { get; }
    bool CanRefresh { get; }
}
