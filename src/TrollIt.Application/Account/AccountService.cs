using TrollIt.Application.Account.Abstractions;
using TrollIt.Application.Account.Models;
using TrollIt.Domain.Accounts.Acl.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;
using TrollIt.Domain.Accounts.Infrastructure;
using TrollIt.Domain.Bestiaries.Infrastructure;
using TrollIt.Domain.Profiles.Infrastructure;

namespace TrollIt.Application.Account;

internal class AccountService(IAccountsAcl accountAcl, IAccountsRepository accountsRepository, ITrollBestiary trollBestiary, IProfilesRepository profilesRepository)
    : IAccountService
{
    public async Task<AccountResponse> CreateAccountAsync(CreateAccountRequest accountRequest)
    {
        var trollInfos = await trollBestiary.GetTroll(accountRequest.TrollId)
            ?? throw new AppException<AccountExceptions>(AccountExceptions.TrollUnknown);

        var trollDto = new TrollDto(accountRequest.TrollId, trollInfos.Name, accountRequest.Token);
        var accountDto = new AccountDto(Guid.NewGuid(), accountRequest.UserName, trollDto);
        var account = accountAcl.ToDomain(accountDto, accountRequest.Password);

        await profilesRepository.RefreshProfileAsync(account.Troll.Id, account.Troll.ScriptToken);

        await accountsRepository.CreateAccount(account);

        return new AccountResponse(account);
    }

    public async Task<AccountResponse?> AuthenticateAsync(AuthenticateRequest authenticateRequest)
    {
        var account = await accountsRepository.GetAccountByLogin(authenticateRequest.UserName);

        if (account == null)
        {
            return null;
        }
        
        return account.ValidateCredentials(authenticateRequest.Password) ? new AccountResponse(account) : null;
    }

    public async Task<AccountResponse> GetAccountAsync(Guid accountId)
    {
        var account = await accountsRepository.GetAccount(accountId);

        return account == null
            ? throw new AppException<AccountExceptions>(AccountExceptions.AccountNotFound)
            : new AccountResponse(account);
    }
}