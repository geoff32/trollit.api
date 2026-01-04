using FluentAssertions;
using NSubstitute;
using TrollIt.Domain.Accounts;
using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Accounts.Acl;
using TrollIt.Domain.Accounts.Acl.Abstractions;
using TrollIt.Domain.Accounts.Infrastructure;
using TrollIt.Infrastructure.Accounts.Acl;
using TrollIt.Infrastructure.Shares.Models;

namespace TrollIt.Infrastructure.Tests.Accounts.Acl;

public class AccountsRepositoryAclTest
{
    private readonly IPasswordEncryptor _passwordEncryptor;

    public AccountsRepositoryAclTest()
    {
        _passwordEncryptor = Substitute.For<IPasswordEncryptor>();
    }

    [Theory]
    [InlineData(FeatureStatus.Inactive, FeatureStatus.Inactive)]
    [InlineData(FeatureStatus.Inactive, FeatureStatus.Readwrite)]
    [InlineData(FeatureStatus.Inactive, FeatureStatus.Read)]
    [InlineData(FeatureStatus.Readwrite, FeatureStatus.Inactive)]
    [InlineData(FeatureStatus.Readwrite, FeatureStatus.Readwrite)]
    [InlineData(FeatureStatus.Readwrite, FeatureStatus.Read)]
    [InlineData(FeatureStatus.Read, FeatureStatus.Inactive)]
    [InlineData(FeatureStatus.Read, FeatureStatus.Readwrite)]
    [InlineData(FeatureStatus.Read, FeatureStatus.Read)]
    internal void ToDomain_ShouldMergeRights_WhenCalledWithTrollIdAndMultiplePolicies(FeatureStatus status1, FeatureStatus status2)
    {
        // Arrange
        var trollId = 1;
        var policy = new Policy(Guid.NewGuid(), "Test Policy", [new TrollShare(2, PolicyStatus.Owner, [new TrollFeature(Shares.Models.FeatureId.Profile, status1)])]);
        var policy2 = new Policy(Guid.NewGuid(), "Test Policy2", [new TrollShare(2, PolicyStatus.Owner, [new TrollFeature(Shares.Models.FeatureId.Profile, status2)])]);
        var policies = new Policy[] { policy, policy2 };

        var accountsRepositoryAcl = new AccountsRepositoryAcl(new AccountsAcl(_passwordEncryptor));

        // Act
        var accountPolicy = accountsRepositoryAcl.ToDomain(trollId, policies);

        // Assert
        accountPolicy.Should().BeOfType<AccountPolicy>()
            .Which.Should().BeEquivalentTo(new AccountPolicy(trollId,
            [
                new TrollRight(2,
                [
                    new Feature(Domain.Accounts.Abstractions.FeatureId.Profile, CanRead(status1) || CanRead(status2), CanRefresh(status1) || CanRefresh(status2)),
                    new Feature(Domain.Accounts.Abstractions.FeatureId.View, false, false)
                ])
            ]));
    }

    internal static bool CanRead(FeatureStatus status) => status == FeatureStatus.Read || status == FeatureStatus.Readwrite;
    internal static bool CanRefresh(FeatureStatus status) => status == FeatureStatus.Readwrite;
}
