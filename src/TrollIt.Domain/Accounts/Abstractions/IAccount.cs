namespace TrollIt.Domain.Accounts.Abstractions;

public interface IAccount
{
    Guid Id { get; }
    string Login { get; }
    IPassword Password { get; }
    ITroll Troll { get; }
}
