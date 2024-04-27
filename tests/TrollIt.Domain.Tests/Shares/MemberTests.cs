using FluentAssertions;
using TrollIt.Domain.Shares;
using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;

namespace TrollIt.Domain.Tests.Shares;

public class MemberTests
{
    [Fact]
    public void Constructor_ShouldSetProperties_WhenCalledWithMemberDto()
    {
        // Arrange
        var memberDto = new MemberDto(1, ShareStatus.User, [new FeatureDto(FeatureId.Profile, false, false)]);

        // Act
        var member = new Member(memberDto);

        // Assert
        member.Id.Should().Be(memberDto.Id);
        member.Status.Should().Be(memberDto.Status);
        member.Features.Should().ContainSingle().Which.Should().BeEquivalentTo(new Feature(FeatureId.Profile, false, false));
    }

    [Fact]
    public void IsGuest_ShouldReturnTrue_WhenStatusIsGuest()
    {
        // Arrange
        var member = new Member(1, ShareStatus.Guest, []);

        // Act
        var isGuest = member.IsGuest;

        // Assert
        isGuest.Should().BeTrue();
    }

    [Fact]
    public void IsGuest_ShouldReturnFalse_WhenStatusIsNotGuest()
    {
        // Arrange
        var member = new Member(1, ShareStatus.User, []);

        // Act
        var isGuest = member.IsGuest;

        // Assert
        isGuest.Should().BeFalse();
    }
}