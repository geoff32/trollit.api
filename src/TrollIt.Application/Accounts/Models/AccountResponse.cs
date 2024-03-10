using TrollIt.Domain.Accounts.Abstractions;

namespace TrollIt.Application.Accounts.Models;

public record AccountResponse(Guid UserId, string UserName, TrollResponse Troll)
{
    public AccountResponse(IAccount account)
        : this(account.Id, account.Login, new TrollResponse(account.Troll))
    {
    }
}
