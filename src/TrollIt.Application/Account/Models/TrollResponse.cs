using TrollIt.Domain.Accounts.Abstractions;

namespace TrollIt.Application.Account.Models;

public record TrollResponse(int Id, string Name)
{
    public TrollResponse(ITroll troll)
        : this(troll.Id, troll.Name)
    {
    }
}
