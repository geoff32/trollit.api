using TrollIt.Application.Account.Abstractions;
using TrollIt.Application.Account.Models;

namespace TrollIt.Application.Account;

internal class AccountService : IAccountService
{
    public Task<AccountResponse> CreateAccountAsync(CreateAccountRequest accountRequest)
    {
        var account = new AccountResponse(Guid.NewGuid(), "Gérard", new(112729, "Gérard Manmalle O'Khrane"));

        return Task.FromResult(account);
    }

    public Task<AccountResponse> AuthenticateAsync(AuthenticateRequest authenticateRequest)
    {
        var account = new AccountResponse(Guid.NewGuid(), "Gérard", new(112729, "Gérard Manmalle O'Khrane"));

        return Task.FromResult(account);
    }

    public Task<AccountResponse> GetAccountAsync(Guid accountId)
    {
        var account = new AccountResponse(accountId, "Gérard", new(112729, "Gérard Manmalle O'Khrane"));

        return Task.FromResult(account);
    }
}