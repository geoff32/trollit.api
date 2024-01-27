using TrollIt.Application.Account.Abstractions;
using TrollIt.Application.Account.Models;
using TrollIt.Domain.Accounts.Acl.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;
using TrollIt.Domain.Accounts.Infrastructure.Abstractions;

namespace TrollIt.Application.Account;

internal class AccountService(IAccountAcl accountAcl, IAccountRepository accountRepository) : IAccountService
{
    public async Task<AccountResponse> CreateAccountAsync(CreateAccountRequest accountRequest)
    {
        var trollDto = new TrollDto(accountRequest.TrollId, "Name", accountRequest.Token);
        var accountDto = new AccountDto(Guid.NewGuid(), accountRequest.UserName, trollDto);
        var account = accountAcl.ToAccount(accountDto, accountRequest.Password);

        await accountRepository.CreateAccount(account);

        return new AccountResponse(account);
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