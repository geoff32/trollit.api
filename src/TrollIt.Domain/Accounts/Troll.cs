using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;

namespace TrollIt.Domain.Accounts;

internal record Troll(int Id, string Name, string ScriptToken) : ITroll
{
    public Troll(TrollDto troll)
        : this(troll.Id, troll.Name, troll.ScriptToken)
    {
    }
}
