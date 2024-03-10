using TrollIt.Application.Accounts.Models;

namespace TrollIt.Application.Accounts.Abstractions;

public interface IAccountsService
{
    Task<AccountResponse> CreateAccountAsync(CreateAccountRequest accountRequest, CancellationToken cancellationToken);
    Task<AccountResponse?> AuthenticateAsync(AuthenticateRequest authenticateReques, CancellationToken cancellationTokent);
    Task<AccountResponse> GetAccountAsync(Guid accountId, CancellationToken cancellationToken);
}