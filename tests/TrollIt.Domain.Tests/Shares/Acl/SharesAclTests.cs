using NSubstitute;
using FluentAssertions;
using TrollIt.Domain.Shares.Acl.Models;
using TrollIt.Domain.Shares.Acl;
using TrollIt.Domain.Shares;
using TrollIt.Domain.Shares.Abstractions;

namespace TrollIt.Domain.Tests.Shares.Acl;

public class SharesAclTests
{
    [Fact]
    public void ToDomain_ShouldReturnSharePolicy_WhenCalledWithSharePolicyDto()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Policy";
        var featureProfile = new FeatureDto(FeatureId.Profile, true, true);
        var featureView = new FeatureDto(FeatureId.View, true, true);
        var sharePolicyDto = new SharePolicyDto(id, name, [new MemberDto(1, ShareStatus.Owner, [featureProfile, featureView])]);
        var sharesAcl = new SharesAcl();

        // Act
        var sharePolicy = sharesAcl.ToDomain(sharePolicyDto);

        // Assert
        sharePolicy.Should().BeOfType<SharePolicy>()
            .Which.Should().BeEquivalentTo(sharePolicyDto);
    }
    [Fact]
    public void ToDomain_ShouldReturnSharePolicyWithAllFeatures_WhenCalledWithSharePolicyDtoWithoutFeature()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Policy";
        var sharePolicyDto = new SharePolicyDto(id, name, [new MemberDto(1, ShareStatus.Owner, [])]);
        var sharesAcl = new SharesAcl();

        // Act
        var sharePolicy = sharesAcl.ToDomain(sharePolicyDto);

        // Assert
        var expectedFeatures = new [] {new Feature(FeatureId.Profile, false, false), new Feature(FeatureId.View, false, false)};
        sharePolicy.Should().BeOfType<SharePolicy>()
            .Which.Should().BeEquivalentTo(new SharePolicy(id, name, [new Member(1, ShareStatus.Owner, expectedFeatures)]));
    }

    [Fact]
    public void ToDomain_ShouldReturnUserPolicy_WhenCalledWithTrollIdAndSharePolicies()
    {
        // Arrange
        var trollId = 1;
        var sharePolicy = new SharePolicy(Guid.NewGuid(), "Test Policy", [new Member(2, ShareStatus.Owner, [new Feature(FeatureId.Profile, true, true)])]);
        var sharePolicies = new ISharePolicy[] { sharePolicy };
        var sharesAcl = new SharesAcl();

        // Act
        var userPolicy = sharesAcl.ToDomain(trollId, sharePolicies);

        // Assert
        userPolicy.Should().BeOfType<UserPolicy>()
            .Which.Should().BeEquivalentTo(new UserPolicy(trollId, [new TrollRight(2, [new Feature(FeatureId.Profile, true, true), new Feature(FeatureId.View, false, false)])]));
    }

    [Theory]
    [InlineData(false, false, false, false)]
    [InlineData(false, false, false, true)]
    [InlineData(false, false, true, false)]
    [InlineData(false, false, true, true)]
    [InlineData(false, true, false, false)]
    [InlineData(false, true, false, true)]
    [InlineData(false, true, true, false)]
    [InlineData(false, true, true, true)]
    [InlineData(true, false, false, false)]
    [InlineData(true, false, false, true)]
    [InlineData(true, false, true, false)]
    [InlineData(true, false, true, true)]
    [InlineData(true, true, false, false)]
    [InlineData(true, true, false, true)]
    [InlineData(true, true, true, false)]
    [InlineData(true, true, true, true)]
    public void ToDomain_ShouldMergeRights_WhenCalledWithTrollIdAndMultipleSharePolicies(bool canRead1, bool canRefresh1, bool canRead2, bool canRefresh2)
    {
        // Arrange
        var trollId = 1;
        var sharePolicy = new SharePolicy(Guid.NewGuid(), "Test Policy", [new Member(2, ShareStatus.Owner, [new Feature(FeatureId.Profile, canRead1, canRefresh1)])]);
        var sharePolicy2 = new SharePolicy(Guid.NewGuid(), "Test Policy2", [new Member(2, ShareStatus.Owner, [new Feature(FeatureId.Profile, canRead2, canRefresh2)])]);
        var sharePolicies = new ISharePolicy[] { sharePolicy, sharePolicy2 };
        var sharesAcl = new SharesAcl();

        // Act
        var userPolicy = sharesAcl.ToDomain(trollId, sharePolicies);

        // Assert
        userPolicy.Should().BeOfType<UserPolicy>()
            .Which.Should().BeEquivalentTo(new UserPolicy(trollId,
            [
                new TrollRight(2,
                [
                    new Feature(FeatureId.Profile, canRead1 || canRead2, canRefresh1 || canRefresh2),
                    new Feature(FeatureId.View, false, false)
                ])
            ]));
    }
}