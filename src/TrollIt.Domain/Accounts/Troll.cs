using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;

namespace TrollIt.Domain.Accounts;

internal class Troll(int id, string name, string scriptToken) : ITroll
{
    public Troll(TrollDto troll)
        : this(troll.Id, troll.Name, troll.ScriptToken)
    {
    }

    public int Id { get; } = id;

    public string Name { get; } = name;

    public string ScriptToken { get; } = scriptToken;
}
