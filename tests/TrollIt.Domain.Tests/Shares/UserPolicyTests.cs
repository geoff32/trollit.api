using NSubstitute;
using FluentAssertions;
using TrollIt.Domain.Shares.Acl.Models;
using TrollIt.Domain.Shares;
using TrollIt.Domain.Shares.Abstractions;
using FluentAssertions.Specialized;
using TrollIt.Domain.Shares.Exceptions;

namespace TrollIt.Domain.Tests.Shares;

public class UserPolicyTests
{
    [Fact]
    public void Constructor_ShouldSetProperties_WhenCalledWithUserPolicyDto()
    {
        // Arrange
        var userPolicyDto = new UserPolicyDto(1, [new TrollRightDto(1, [])]);

        // Act
        var userPolicy = new UserPolicy(userPolicyDto);

        // Assert
        userPolicy.TrollId.Should().Be(userPolicyDto.Id);
        userPolicy.Rights.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new TrollRight(1, []));
    }

    [Fact]
    public void CanRead_ShouldReturnTrue_WhenReadOwnTroll()
    {
        // Arrange
        var featureId = FeatureId.Profile;
        var trollId = 1;
        var userPolicy = new UserPolicy(trollId, []);

        // Act
        var canRead = userPolicy.CanRead(featureId, trollId);

        // Assert
        canRead.Should().BeTrue();
    }

    [Fact]
    public void CanRead_ShouldReturnFalse_WhenCannotReadOtherTroll()
    {
        // Arrange
        var featureId = FeatureId.Profile;
        var trollId = 1;
        var feature = Substitute.For<IFeature>();
        feature.Id.Returns(featureId);
        feature.CanRead.Returns(false);
        var trollRight = Substitute.For<ITrollRight>();
        trollRight.TrollId.Returns(trollId);
        trollRight.Features.Returns([feature]);
        var userPolicy = new UserPolicy(2, [trollRight]);

        // Act
        var canRead = userPolicy.CanRead(featureId, trollId);

        // Assert
        canRead.Should().BeFalse();
    }

    [Fact]
    public void CanRefresh_ShouldReturnTrue_WhenRefreshOwnTroll()
    {
        // Arrange
        var featureId = FeatureId.Profile;
        var trollId = 1;
        var userPolicy = new UserPolicy(trollId, []);

        // Act
        var canRefresh = userPolicy.CanRefresh(featureId, trollId);

        // Assert
        canRefresh.Should().BeTrue();
    }

    [Fact]
    public void CanRefresh_ShouldReturnTrue_WhenCanRefreshOtherTroll()
    {
        // Arrange
        var featureId = FeatureId.Profile;
        var trollId = 1;
        var feature = Substitute.For<IFeature>();
        feature.Id.Returns(featureId);
        feature.CanRefresh.Returns(true);
        var trollRight = Substitute.For<ITrollRight>();
        trollRight.TrollId.Returns(trollId);
        trollRight.Features.Returns([feature]);
        var userPolicy = new UserPolicy(2, [trollRight]);

        // Act
        var canRefresh = userPolicy.CanRefresh(featureId, trollId);

        // Assert
        canRefresh.Should().BeTrue();
    }

    [Fact]
    public void CanRefresh_ShouldReturnFalse_WhenCannotRefreshOtherTroll()
    {
        // Arrange
        var featureId = FeatureId.Profile;
        var trollId = 1;
        var feature = Substitute.For<IFeature>();
        feature.Id.Returns(featureId);
        feature.CanRefresh.Returns(false);
        var trollRight = Substitute.For<ITrollRight>();
        trollRight.Features.Returns([feature]);
        var userPolicy = new UserPolicy(2, [trollRight]);

        // Act
        var canRefresh = userPolicy.CanRefresh(featureId, trollId);

        // Assert
        canRefresh.Should().BeFalse();
    }
}