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
    public void ToDomain_ShouldReturnPolicy_WhenCalledWithPolicyDto()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Policy";
        var featureProfile = new FeatureDto(FeatureId.Profile, true, true);
        var featureView = new FeatureDto(FeatureId.View, true, true);
        var policyDto = new PolicyDto(id, name, [new MemberDto(1, PolicyStatus.Owner, [featureProfile, featureView])]);
        var sharesAcl = new SharesAcl();

        // Act
        var policy = sharesAcl.ToDomain(policyDto);

        // Assert
        policy.Should().BeOfType<Policy>()
            .Which.Should().BeEquivalentTo(policyDto);
    }
    [Fact]
    public void ToDomain_ShouldReturnPolicyWithAllFeatures_WhenCalledWithPolicyDtoWithoutFeature()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Policy";
        var policyDto = new PolicyDto(id, name, [new MemberDto(1, PolicyStatus.Owner, [])]);
        var sharesAcl = new SharesAcl();

        // Act
        var policy = sharesAcl.ToDomain(policyDto);

        // Assert
        var expectedFeatures = new [] {new Feature(FeatureId.Profile, false, false), new Feature(FeatureId.View, false, false)};
        policy.Should().BeOfType<Policy>()
            .Which.Should().BeEquivalentTo(new Policy(id, name, [new Member(1, PolicyStatus.Owner, expectedFeatures)]));
    }
}