namespace TrollIt.Domain.Shares.Abstractions;

public interface IMember
{
    int Id { get; }
    PolicyStatus Status { get; }
    IEnumerable<IFeature> Features { get; }
    bool IsGuest { get; }
}
