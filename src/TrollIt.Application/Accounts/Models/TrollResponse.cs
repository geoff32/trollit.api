using TrollIt.Domain.Accounts.Abstractions;

namespace TrollIt.Application.Accounts.Models;

public record TrollResponse(int Id, string Name)
{
    public TrollResponse(ITroll troll)
        : this(troll.Id, troll.Name)
    {
    }
}
