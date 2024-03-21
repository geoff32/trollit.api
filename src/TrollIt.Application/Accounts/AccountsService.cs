using TrollIt.Application.Accounts.Abstractions;
using TrollIt.Application.Accounts.Exceptions;
using TrollIt.Application.Accounts.Models;
using TrollIt.Domain.Accounts.Acl.Abstractions;
using TrollIt.Domain.Accounts.Acl.Models;
using TrollIt.Domain.Accounts.Infrastructure;
using TrollIt.Domain.Bestiaries.Infrastructure;
using TrollIt.Domain.Profiles.Infrastructure;

namespace TrollIt.Application.Accounts;

internal class AccountsService
(
    IAccountsAcl accountAcl,
    IPasswordEncryptor passwordEncryptor,
    IAccountsRepository accountsRepository,
    ITrollBestiary trollBestiary,
    IProfilesRepository profilesRepository
) : IAccountsService
{
    public async Task<AccountResponse> CreateAccountAsync(CreateAccountRequest accountRequest, CancellationToken cancellationToken)
    {
        var trollInfos = await trollBestiary.GetTrollAsync(accountRequest.TrollId)
            ?? throw new AppException<AccountExceptions>(AccountExceptions.TrollUnknown);

        var existingAccount = await accountsRepository.GetAccountByLoginAsync(accountRequest.UserName, cancellationToken);
        if (existingAccount != null)
        {
            throw new AppException<AccountExceptions>(AccountExceptions.AccountAlreadyExists);
        }

        var trollDto = new TrollDto(accountRequest.TrollId, trollInfos.Name, accountRequest.Token);
        var accountDto = new AccountDto(Guid.NewGuid(), accountRequest.UserName, trollDto);
        var account = accountAcl.ToDomain(accountDto, accountRequest.Password);

        await profilesRepository.RefreshProfileAsync(account.Troll.Id, account.Troll.ScriptToken, cancellationToken);

        await accountsRepository.CreateAccountAsync(account, cancellationToken);

        return new AccountResponse(account);
    }

    public async Task<AccountResponse?> AuthenticateAsync(AuthenticateRequest authenticateRequest, CancellationToken cancellationToken)
    {
        var account = await accountsRepository.GetAccountByLoginAsync(authenticateRequest.UserName, cancellationToken);

        if (account == null)
        {
            return null;
        }

        var hashedPassword = passwordEncryptor.Encrypt(authenticateRequest.Password, account.GetSalt());
        return account.ValidateCredentials(hashedPassword)
            ? new AccountResponse(account)
            : null;
    }

    public async Task<AccountResponse> GetAccountAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var account = await accountsRepository.GetAccountAsync(accountId, cancellationToken);

        return account == null
            ? throw new AppException<AccountExceptions>(AccountExceptions.AccountNotFound)
            : new AccountResponse(account);
    }
}