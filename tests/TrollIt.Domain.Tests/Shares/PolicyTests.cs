using FluentAssertions;
using FluentAssertions.Specialized;
using NSubstitute;
using TrollIt.Domain.Shares;
using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;
using TrollIt.Domain.Shares.Exceptions;

namespace TrollIt.Domain.Tests.Shares;

public class PolicyTests
{
    [Fact]
    public void AddInvitation_ShouldAddInvitation_WhenMemberDoesNotExist()
    {
        // Arrange
        var memberId = 1;
        var policyDto = new PolicyDto(Guid.NewGuid(), "testName", []);
        var policy = new Policy(policyDto);
        var invitation = new Invitation(policy.Id, memberId, []);

        // Act
        policy.AddInvitation(invitation);

        // Assert
        policy.Members.Should().ContainSingle(m => m.Id == memberId && m.Status == PolicyStatus.Guest);
    }

    [Theory]
    [InlineData(PolicyStatus.Owner)]
    [InlineData(PolicyStatus.Admin)]
    [InlineData(PolicyStatus.User)]
    public void AddInvitation_ShouldThrowException_WhenAlreadyMember(PolicyStatus policyStatus)
    {
        // Arrange
        var memberId = 1;
        var memberDto = new MemberDto(memberId, policyStatus, []);
        var policyDto = new PolicyDto(Guid.NewGuid(), "testName", [memberDto]);
        var policy = new Policy(policyDto);
        var invitation = new Invitation(policy.Id, memberId, []);

        // Act
        var act = () => policy.AddInvitation(invitation);

        // Assert
        act.Should().ThrowDomainException(SharesExceptions.IsAlreadyMember);
    }

    [Fact]
    public void AddInvitation_ShouldThrowException_WhenInvitationAlreadyExists()
    {
        // Arrange
        const int memberId = 1;
        var memberDto = new MemberDto(memberId, PolicyStatus.Guest, []);
        var policyDto = new PolicyDto(Guid.NewGuid(), "testName", [memberDto]);
        var policy = new Policy(policyDto);
        var invitation = new Invitation(policy.Id, memberId, []);

        // Act
        var act = () => policy.AddInvitation(invitation);

        // Assert
        act.Should().ThrowDomainException(SharesExceptions.InvitationAlreadyExistsInPolicy);
    }

    [Fact]
    public void AcceptInvitation_ShouldChangeStatusToUser_WhenInvitationExists()
    {
        // Arrange
        const int memberId = 1;
        var member = Substitute.For<IMember>();
        member.Id.Returns(memberId);
        member.IsGuest.Returns(true);
        var policy = new Policy(Guid.NewGuid(), "testName", [member]);
        var invitation = new Invitation(policy.Id, memberId, []);

        // Act
        policy.AcceptInvitation(invitation);

        // Assert
        policy.Members.Should().ContainSingle(m => m.Id == memberId && m.Status == PolicyStatus.User);
    }

    [Fact]
    public void AcceptInvitation_ShouldThrowException_WhenInvitationDoesNotExist()
    {
        // Arrange
        const int memberId = 1;
        var policy = new Policy(Guid.NewGuid(), "testName", []);
        var invitation = new Invitation(policy.Id, memberId, []);

        // Act
        var act = () => policy.AcceptInvitation(invitation);

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
        var policy = new Policy(Guid.NewGuid(), "testName", [member]);
        var invitation = new Invitation(policy.Id, memberId, []);

        // Act
        var act = () => policy.AcceptInvitation(invitation);

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
        var policy = new Policy(Guid.NewGuid(), "testName", [member]);

        // Act
        policy.RemoveMember(memberId);

        // Assert
        policy.Members.Should().NotContain(member);
    }
    
    [Fact]
    public void RemoveMember_ShouldThrowException_WhenNotMember()
    {
        // Arrange
        var memberId = 1;
        var policy = new Policy(Guid.NewGuid(), "testName", []);

        // Act
        var act = () => policy.RemoveMember(memberId);

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
        var policy = new Policy(Guid.NewGuid(), "testName", [member]);

        // Act
        var act = () => policy.RemoveMember(memberId);

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
        var policy = new Policy(Guid.NewGuid(), "testName", [member]);
        var invitation = new Invitation(policy.Id, memberId, []);

        // Act
        policy.RemoveInvitation(invitation);

        // Assert
        policy.Members.Should().NotContain(member);
    }
    
    [Fact]
    public void RemoveInvitation_ShouldThrowException_WhenMemberIsAlreadyMember()
    {
        // Arrange
        var memberId = 1;
        var member = Substitute.For<IMember>();
        member.Id.Returns(memberId);
        member.IsGuest.Returns(false);
        var policy = new Policy(Guid.NewGuid(), "testName", [member]);
        var invitation = new Invitation(policy.Id, memberId, []);

        // Act
        var act = () => policy.RemoveInvitation(invitation);

        // Assert
        act.Should().ThrowDomainException(SharesExceptions.IsAlreadyMember);
    }
}