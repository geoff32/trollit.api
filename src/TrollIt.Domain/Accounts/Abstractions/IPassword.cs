namespace TrollIt.Domain.Accounts.Abstractions;

public interface IPassword : IEquatable<IPassword>
{
    public IEnumerable<byte> Value { get; }
}
