using TrollIt.Application.Account.Abstractions;
using TrollIt.Application.Account.Models;
using TrollIt.Domain.Accounts.Acl.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;
using TrollIt.Domain.Accounts.Infrastructure.Abstractions;
using TrollIt.Domain.Bestiaries.Infrastructure.Abstractions;

namespace TrollIt.Application.Account;

internal class AccountService(IAccountAcl accountAcl, IAccountRepository accountRepository, ITrollBestiary trollBestiary)
    : IAccountService
{
    public async Task<AccountResponse> CreateAccountAsync(CreateAccountRequest accountRequest)
    {
        var trollInfos = await trollBestiary.GetTroll(accountRequest.TrollId)
            ?? throw new AppException<AccountExceptions>(AccountExceptions.TrollUnknown);

        var trollDto = new TrollDto(accountRequest.TrollId, trollInfos.Name, accountRequest.Token);
        var accountDto = new AccountDto(Guid.NewGuid(), accountRequest.UserName, trollDto);
        var account = accountAcl.ToAccount(accountDto, accountRequest.Password);

        await accountRepository.CreateAccount(account);

        return new AccountResponse(account);
    }

    public async Task<AccountResponse?> AuthenticateAsync(AuthenticateRequest authenticateRequest)
    {
        var account = await accountRepository.GetAccountByLogin(authenticateRequest.UserName);

        if (account == null)
        {
            return null;
        }
        
        return account.ValidateCredentials(authenticateRequest.Password) ? new AccountResponse(account) : null;
    }

    public async Task<AccountResponse> GetAccountAsync(Guid accountId)
    {
        var account = await accountRepository.GetAccount(accountId);

        return account == null
            ? throw new AppException<AccountExceptions>(AccountExceptions.AccountNotFound)
            : new AccountResponse(account);
    }
}