using TrollIt.Domain.Accounts.Infrastructure.Abstractions;

namespace TrollIt.Domain.Accounts.Acl.Models;

public record class AccountDto(Guid Id, string Login, TrollDto Troll);