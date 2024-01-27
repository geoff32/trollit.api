using TrollIt.Domain.Accounts.Infrastructure.Abstractions;

namespace TrollIt.Domain.Accounts.Acl.Models;

public record class NewAccountDto(string Login, string Password, TrollDto Troll);