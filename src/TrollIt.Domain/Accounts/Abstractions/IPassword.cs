namespace TrollIt.Domain.Accounts.Abstractions;

public interface IPassword
{
    public IEnumerable<byte> Value { get; }
}
