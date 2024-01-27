using TrollIt.Domain.Accounts.Abstractions;

namespace TrollIt.Domain.Accounts.Infrastructure.Abstractions;

public interface IAccountRepository
{
    Task CreateAccount(IAccount account);
}
