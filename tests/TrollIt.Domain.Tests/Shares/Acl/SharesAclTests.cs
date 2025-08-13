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
}