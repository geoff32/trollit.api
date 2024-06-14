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
        userPolicy.Rights.Should().ContainSingle().Which.Should().BeEquivalentTo(new TrollRight(1, []));
    }

    [Fact]
    public void EnsureReadAccess_ShouldNotThrow_WhenReadOwnTroll()
    {
        // Arrange
        var featureId = FeatureId.Profile;
        var trollId = 1;
        var userPolicy = new UserPolicy(trollId, []);

        // Act
        Action act = () => userPolicy.EnsureReadAccess(featureId, trollId);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void EnsureReadAccess_ShouldNotThrow_WhenCanReadOtherTroll()
    {
        // Arrange
        var featureId = FeatureId.Profile;
        var trollId = 1;
        var feature = Substitute.For<IFeature>();
        feature.Id.Returns(featureId);
        feature.CanRead.Returns(true);
        var trollRight = Substitute.For<ITrollRight>();
        trollRight.TrollId.Returns(trollId);
        trollRight.Features.Returns([feature]);
        var userPolicy = new UserPolicy(2, [trollRight]);

        // Act
        Action act = () => userPolicy.EnsureReadAccess(featureId, trollId);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void EnsureReadAccess_ShouldThrow_WhenCannotReadOtherTroll()
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
        Action act = () => userPolicy.EnsureReadAccess(featureId, trollId);

        // Assert
        act.Should().ThrowDomainException(SharesExceptions.NoReadAccess);
    }

    [Fact]
    public void EnsureRefreshAccess_ShouldNotThrow_WhenRefreshOwnTroll()
    {
        // Arrange
        var featureId = FeatureId.Profile;
        var trollId = 1;
        var userPolicy = new UserPolicy(trollId, []);

        // Act
        Action act = () => userPolicy.EnsureRefreshAccess(featureId, trollId);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void EnsureRefreshAccess_ShouldNotThrow_WhenCanRefreshOtherTroll()
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
        Action act = () => userPolicy.EnsureRefreshAccess(featureId, trollId);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void EnsureRefreshAccess_ShouldThrow_WhenCannotRefreshOtherTroll()
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
        Action act = () => userPolicy.EnsureRefreshAccess(featureId, trollId);

        // Assert
        act.Should().ThrowDomainException(SharesExceptions.NoRefreshAccess);
    }
}