namespace TrollIt.Domain.Shares.Abstractions;

public interface IInvitation
{
    Guid PolicyId { get; }
    int TrollId { get; }
    IReadOnlyCollection<IFeature> Features { get; }
}