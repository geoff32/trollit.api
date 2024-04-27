using FluentAssertions;
using FluentAssertions.Specialized;
using NSubstitute;
using TrollIt.Domain.Shares;
using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;
using TrollIt.Domain.Shares.Exceptions;

namespace TrollIt.Domain.Tests.Shares;

public class SharePolicyTests
{
    [Fact]
    public void AddInvitation_ShouldAddInvitation_WhenMemberDoesNotExist()
    {
        // Arrange
        var memberId = 1;
        var sharePolicyDto = new SharePolicyDto(Guid.NewGuid(), "testName", []);
        var sharePolicy = new SharePolicy(sharePolicyDto);

        // Act
        sharePolicy.AddInvitation(memberId);

        // Assert
        sharePolicy.Members.Should().ContainSingle(m => m.Id == memberId && m.Status == ShareStatus.Guest);
    }

    [Theory]
    [InlineData(ShareStatus.Owner)]
    [InlineData(ShareStatus.Admin)]
    [InlineData(ShareStatus.User)]
    public void AddInvitation_ShouldThrowException_WhenAlreadyMember(ShareStatus shareStatus)
    {
        // Arrange
        var memberId = 1;
        var memberDto = new MemberDto(memberId, shareStatus, []);
        var sharePolicyDto = new SharePolicyDto(Guid.NewGuid(), "testName", [memberDto]);
        var sharePolicy = new SharePolicy(sharePolicyDto);

        // Act
        Action act = () => sharePolicy.AddInvitation(memberId);

        // Assert
        act.Should().ThrowDomainException(SharesExceptions.IsAlreadyMember);
    }

    [Fact]
    public void AddInvitation_ShouldThrowException_WhenInvitationAlreadyExists()
    {
        // Arrange
        var memberId = 1;
        var memberDto = new MemberDto(memberId, ShareStatus.Guest, []);
        var sharePolicyDto = new SharePolicyDto(Guid.NewGuid(), "testName", [memberDto]);
        var sharePolicy = new SharePolicy(sharePolicyDto);

        // Act
        Action act = () => sharePolicy.AddInvitation(memberId);

        // Assert
        act.Should().ThrowDomainException(SharesExceptions.InvitationAlreadyExistsInSharePolicy);
    }

    [Fact]
    public void AcceptInvitation_ShouldChangeStatusToUser_WhenInvitationExists()
    {
        // Arrange
        var memberId = 1;
        var member = Substitute.For<IMember>();
        member.Id.Returns(memberId);
        member.IsGuest.Returns(true);
        var sharePolicy = new SharePolicy(Guid.NewGuid(), "testName", [member]);

        // Act
        sharePolicy.AcceptInvitation(memberId);

        // Assert
        sharePolicy.Members.Should().ContainSingle(m => m.Id == memberId && m.Status == ShareStatus.User);
    }

    [Fact]
    public void AcceptInvitation_ShouldThrowException_WhenInvitationDoesNotExist()
    {
        // Arrange
        var memberId = 1;
        var sharePolicy = new SharePolicy(Guid.NewGuid(), "testName", []);

        // Act
        Action act = () => sharePolicy.AcceptInvitation(memberId);

        // Assert
        act.Should().ThrowDomainException(SharesExceptions.InvitationNotFound);
    }

    [Fact]
    public void AcceptInvitation_ShouldThrowException_WhenAlreadyMember()
    {
        // Arrange
        var memberId = 1;
        var member = Substitute.For<IMember>();
        member.Id.Returns(memberId);
        member.IsGuest.Returns(false);
        var sharePolicy = new SharePolicy(Guid.NewGuid(), "testName", [member]);

        // Act
        Action act = () => sharePolicy.AcceptInvitation(memberId);

        // Assert
        act.Should().ThrowDomainException(SharesExceptions.IsAlreadyMember);
    }

    [Fact]
    public void RemoveMember_ShouldRemoveMember_WhenMemberExists()
    {
        // Arrange
        var memberId = 1;
        var member = Substitute.For<IMember>();
        member.Id.Returns(memberId);
        member.IsGuest.Returns(false);
        var sharePolicy = new SharePolicy(Guid.NewGuid(), "testName", [member]);

        // Act
        sharePolicy.RemoveMember(memberId);

        // Assert
        sharePolicy.Members.Should().NotContain(member);
    }
    
    [Fact]
    public void RemoveMember_ShouldThrowException_WhenNotMember()
    {
        // Arrange
        var memberId = 1;
        var sharePolicy = new SharePolicy(Guid.NewGuid(), "testName", []);

        // Act
        Action act = () => sharePolicy.RemoveMember(memberId);

        // Assert
        act.Should().ThrowDomainException(SharesExceptions.MemberNotFound);
    }
    
    [Fact]
    public void RemoveMember_ShouldThrowException_WhenMemberIsGuest()
    {
        // Arrange
        var memberId = 1;
        var member = Substitute.For<IMember>();
        member.Id.Returns(memberId);
        member.IsGuest.Returns(true);
        var sharePolicy = new SharePolicy(Guid.NewGuid(), "testName", [member]);

        // Act
        Action act = () => sharePolicy.RemoveMember(memberId);

        // Assert
        act.Should().ThrowDomainException(SharesExceptions.MemberNotFound);
    }

    [Fact]
    public void RemoveInvitation_ShouldRemoveInvitation_WhenInvitationExists()
    {
        // Arrange
        var memberId = 1;
        var member = Substitute.For<IMember>();
        member.Id.Returns(memberId);
        member.IsGuest.Returns(true);
        var sharePolicy = new SharePolicy(Guid.NewGuid(), "testName", [member]);

        // Act
        sharePolicy.RemoveInvitation(memberId);

        // Assert
        sharePolicy.Members.Should().NotContain(member);
    }
    
    [Fact]
    public void RemoveInvitation_ShouldThrowException_WhenMemberIsAlreadyMember()
    {
        // Arrange
        var memberId = 1;
        var member = Substitute.For<IMember>();
        member.Id.Returns(memberId);
        member.IsGuest.Returns(false);
        var sharePolicy = new SharePolicy(Guid.NewGuid(), "testName", [member]);

        // Act
        Action act = () => sharePolicy.RemoveInvitation(memberId);

        // Assert
        act.Should().ThrowDomainException(SharesExceptions.IsAlreadyMember);
    }
}