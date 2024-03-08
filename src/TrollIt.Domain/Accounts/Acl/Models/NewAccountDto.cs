namespace TrollIt.Domain.Accounts.Acl.Models;

public record NewAccountDto(string Login, string Password, TrollDto Troll);