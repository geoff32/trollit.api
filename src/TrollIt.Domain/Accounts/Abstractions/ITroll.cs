namespace TrollIt.Domain.Accounts.Abstractions;

public interface ITroll
{
    int Id { get; }
    string Name { get; }
    string ScriptToken { get; }
}
