using TrollIt.Domain.Accounts.Abstractions;

namespace TrollIt.Domain.Accounts.Infrastructure;

public interface IAccountsRepository
{
    Task CreateAccount(IAccount account);
    Task<IAccount?> GetAccount(Guid id);
    Task<IAccount?> GetAccountByLogin(string login);
}
