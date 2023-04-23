namespace TrollIt.Application.Account.Models;

public record AccountResponse(Guid UserId, string UserName, TrollResponse Troll);