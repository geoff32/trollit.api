using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TrollIt.Application.Shares;
using TrollIt.Application.Shares.Models;
using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;
using TrollIt.Domain.Shares.Infrastructure;

namespace TrollIt.Application.Tests.Shares;

public class SharesServiceTests
{
    private readonly SharesService _sharesService;
    private readonly ISharesRepository _sharesRepository;
    private readonly ISharesAcl _sharesAcl;

    public SharesServiceTests()
    {
        _sharesRepository = Substitute.For<ISharesRepository>();
        _sharesAcl = Substitute.For<ISharesAcl>();
        _sharesService = new SharesService(_sharesRepository, _sharesAcl);
    }

    [Fact]
    public async Task CreatePolicyAsync_ShouldCreatePolicy()
    {
        // Arrange
        var user = new AppUser(Guid.NewGuid(), 1, "accountId");
        var request = new CreatePolicyRequest
        (
            Name: "Test",
            Features: new FeatureSettingsRequest(
                new FeatureRequest(CanRead: true, CanRefresh: true),
                new FeatureRequest(CanRead: true, CanRefresh: true)
            )
        );
        var cancellationToken = new CancellationToken();
        var memberDto = new MemberDto
        (
            Id: user.TrollId,
            Status: PolicyStatus.Owner,
            Features: [
                new FeatureDto(Id: FeatureId.Profile, CanRead: true, CanRefresh: true),
                new FeatureDto(Id: FeatureId.View, CanRead: true, CanRefresh: true)
            ]
        );
        var policyDto = new PolicyDto
        (
            Id: Guid.NewGuid(),
            Name: request.Name,
            Members: [memberDto]
        );
        var expectedPolicy = ToDomain(policyDto);

        _sharesAcl.ToDomain(Arg.Any<PolicyDto>()).Returns(expectedPolicy);

        // Act
        var result = await _sharesService.CreatePolicyAsync(user, request, cancellationToken);

        // Assert
        await _sharesRepository.Received(1).SaveAsync(expectedPolicy, cancellationToken);
        result.Should().BeEquivalentTo(new PolicyResponse(expectedPolicy));
    }

    [Fact]
    public async Task GetPolicyAsync_WhenUserBelongsToPolicyAndIsNotGuest_ReturnsExpectedPolicyResponse()
    {
        // Arrange
        var sharesRepository = Substitute.For<ISharesRepository>();
        var policyId = Guid.NewGuid();
        var cancellationToken = new CancellationToken();
        var user = new AppUser(Guid.NewGuid(), 1, "accountId");
        var policy = Substitute.For<IPolicy>();
        policy.Id.Returns(policyId);
        policy.Name.Returns("Name");
        var member = Substitute.For<IMember>();
        member.IsGuest.Returns(false);
        policy.GetMember(user.TrollId).Returns(member);

        var expectedPolicyResponse = new PolicyResponse(policy);

        sharesRepository.GetPolicyAsync(policyId, cancellationToken).Returns(policy);
        var sharesService = new SharesService(sharesRepository, Substitute.For<ISharesAcl>());

        // Act
        var result = await sharesService.GetPolicyAsync(user, policyId, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(expectedPolicyResponse);
    }

    [Fact]
    public async Task GetPolicyAsync_WhenNotExists_ReturnsNull()
    {
        // Arrange
        var sharesRepository = Substitute.For<ISharesRepository>();
        var policyId = Guid.NewGuid();
        var cancellationToken = new CancellationToken();
        var user = new AppUser(Guid.NewGuid(), 1, "accountId");

        sharesRepository.GetPolicyAsync(policyId, cancellationToken).ReturnsNull();
        var sharesService = new SharesService(sharesRepository, Substitute.For<ISharesAcl>());

        // Act
        var result = await sharesService.GetPolicyAsync(user, policyId, cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetPolicyAsync_WhenUserNotBelongsToPolicy_ReturnsNull()
    {
        // Arrange
        var sharesRepository = Substitute.For<ISharesRepository>();
        var policyId = Guid.NewGuid();
        var cancellationToken = new CancellationToken();
        var user = new AppUser(Guid.NewGuid(), 1, "accountId");
        var policy = Substitute.For<IPolicy>();
        policy.Id.Returns(policyId);
        policy.Name.Returns("Name");
        policy.GetMember(user.TrollId).ReturnsNull();

        sharesRepository.GetPolicyAsync(policyId, cancellationToken).Returns(policy);
        var sharesService = new SharesService(sharesRepository, Substitute.For<ISharesAcl>());

        // Act
        var result = await sharesService.GetPolicyAsync(user, policyId, cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]

    public async Task GetPolicyAsync_WhenUserIsOnlyGuest_ReturnsNulls()
    {
        // Arrange
        var sharesRepository = Substitute.For<ISharesRepository>();
        var policyId = Guid.NewGuid();
        var cancellationToken = new CancellationToken();
        var user = new AppUser(Guid.NewGuid(), 1, "accountId");
        var policy = Substitute.For<IPolicy>();
        policy.Id.Returns(policyId);
        policy.Name.Returns("Name");
        var member = Substitute.For<IMember>();
        member.IsGuest.Returns(true);
        policy.GetMember(user.TrollId).Returns(member);

        sharesRepository.GetPolicyAsync(policyId, cancellationToken).Returns(policy);
        var sharesService = new SharesService(sharesRepository, Substitute.For<ISharesAcl>());

        // Act
        var result = await sharesService.GetPolicyAsync(user, policyId, cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    private static IPolicy ToDomain(PolicyDto policyDto)
    {
        var expectedPolicy = Substitute.For<IPolicy>();
        expectedPolicy.Id.Returns(policyDto.Id);
        expectedPolicy.Name.Returns(policyDto.Name);
        var expectedMembers = policyDto.Members.Select(ToDomain).ToArray();
        expectedPolicy.Members.Returns(expectedMembers);
        return expectedPolicy;
    }

    private static IMember ToDomain(MemberDto memberDto)
    {
        var expectedMember = Substitute.For<IMember>();
        expectedMember.Id.Returns(memberDto.Id);
        expectedMember.Status.Returns(memberDto.Status);
        var expectedFeatures = memberDto.Features.Select(ToDomain).ToArray();
        expectedMember.Features.Returns(expectedFeatures);
        return expectedMember;
    }

    private static IFeature ToDomain(FeatureDto featureDto)
    {
        var expectedFeature = Substitute.For<IFeature>();
        expectedFeature.Id.Returns(featureDto.Id);
        expectedFeature.CanRead.Returns(featureDto.CanRead);
        expectedFeature.CanRefresh.Returns(featureDto.CanRefresh);
        return expectedFeature;
    }
}