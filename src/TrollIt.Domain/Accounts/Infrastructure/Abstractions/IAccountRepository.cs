using TrollIt.Domain.Accounts.Abstractions;

namespace TrollIt.Domain.Accounts.Infrastructure.Abstractions;

public interface IAccountRepository
{
    Task CreateAccount(IAccount account);
    Task<IAccount?> GetAccount(Guid id);
    Task<IAccount?> GetAccountByLogin(string login);
}
