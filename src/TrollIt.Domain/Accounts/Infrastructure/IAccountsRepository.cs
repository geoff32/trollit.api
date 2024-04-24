using TrollIt.Domain.Accounts.Abstractions;

namespace TrollIt.Domain.Accounts.Infrastructure;

public interface IAccountsRepository
{
    Task CreateAccountAsync(IAccount account, CancellationToken cancellationToken);
    Task<IAccount?> GetAccountAsync(Guid id, CancellationToken cancellationToken);
    Task<IAccount?> GetAccountByLoginAsync(string login, CancellationToken cancellationToken);
    Task<IAccount?> GetAccountByTrollAsync(int trollId, CancellationToken cancellationToken);
}
