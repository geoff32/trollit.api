namespace TrollIt.Application.Tests.Accounts.Fixtures;

public class AccountsFixtureBuilder()
{
    public AccountsFixture Build(Action<AccountsFixture>? configure = null)
    {
        var accountsFixture = new AccountsFixture();
        configure?.Invoke(accountsFixture);

        return accountsFixture;
    }
}
