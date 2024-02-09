using TrollIt.Application.Account.Models;

namespace TrollIt.Application.Account.Abstractions;

public interface IAccountService
{
    Task<AccountResponse> CreateAccountAsync(CreateAccountRequest accountRequest);
    Task<AccountResponse?> AuthenticateAsync(AuthenticateRequest authenticateRequest);
    Task<AccountResponse> GetAccountAsync(Guid accountId);
}