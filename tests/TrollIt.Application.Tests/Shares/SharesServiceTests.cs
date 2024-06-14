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
    public async Task CreateSharePolicyAsync_ShouldCreatePolicy()
    {
        // Arrange
        var user = new AppUser(Guid.NewGuid(), 1, "accountId");
        var request = new CreateSharePolicyRequest
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
            Status: ShareStatus.Owner,
            Features: [
                new FeatureDto(Id: FeatureId.Profile, CanRead: true, CanRefresh: true),
                new FeatureDto(Id: FeatureId.View, CanRead: true, CanRefresh: true)
            ]
        );
        var sharePolicyDto = new SharePolicyDto
        (
            Id: Guid.NewGuid(),
            Name: request.Name,
            Members: [memberDto]
        );
        var expectedSharePolicy = ToDomain(sharePolicyDto);

        _sharesAcl.ToDomain(Arg.Any<SharePolicyDto>()).Returns(expectedSharePolicy);

        // Act
        var result = await _sharesService.CreateSharePolicyAsync(user, request, cancellationToken);

        // Assert
        await _sharesRepository.Received(1).SaveAsync(expectedSharePolicy, cancellationToken);
        result.Should().BeEquivalentTo(new SharePolicyResponse(expectedSharePolicy));
    }

    [Fact]
    public async Task GetSharePolicyAsync_WhenUserBelongsToPolicyAndIsNotGuest_ReturnsExpectedSharePolicyResponse()
    {
        // Arrange
        var sharesRepository = Substitute.For<ISharesRepository>();
        var sharePolicyId = Guid.NewGuid();
        var cancellationToken = new CancellationToken();
        var user = new AppUser(Guid.NewGuid(), 1, "accountId");
        var sharePolicy = Substitute.For<ISharePolicy>();
        sharePolicy.Id.Returns(sharePolicyId);
        sharePolicy.Name.Returns("Name");
        var member = Substitute.For<IMember>();
        member.IsGuest.Returns(false);
        sharePolicy.GetMember(user.TrollId).Returns(member);

        var expectedSharePolicyResponse = new SharePolicyResponse(sharePolicy);

        sharesRepository.GetSharePolicyAsync(sharePolicyId, cancellationToken).Returns(sharePolicy);
        var sharesService = new SharesService(sharesRepository, Substitute.For<ISharesAcl>());

        // Act
        var result = await sharesService.GetSharePolicyAsync(user, sharePolicyId, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(expectedSharePolicyResponse);
    }

    [Fact]
    public async Task GetSharePolicyAsync_WhenNotExists_ReturnsNull()
    {
        // Arrange
        var sharesRepository = Substitute.For<ISharesRepository>();
        var sharePolicyId = Guid.NewGuid();
        var cancellationToken = new CancellationToken();
        var user = new AppUser(Guid.NewGuid(), 1, "accountId");

        sharesRepository.GetSharePolicyAsync(sharePolicyId, cancellationToken).ReturnsNull();
        var sharesService = new SharesService(sharesRepository, Substitute.For<ISharesAcl>());

        // Act
        var result = await sharesService.GetSharePolicyAsync(user, sharePolicyId, cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetSharePolicyAsync_WhenUserNotBelongsToPolicy_ReturnsNull()
    {
        // Arrange
        var sharesRepository = Substitute.For<ISharesRepository>();
        var sharePolicyId = Guid.NewGuid();
        var cancellationToken = new CancellationToken();
        var user = new AppUser(Guid.NewGuid(), 1, "accountId");
        var sharePolicy = Substitute.For<ISharePolicy>();
        sharePolicy.Id.Returns(sharePolicyId);
        sharePolicy.Name.Returns("Name");
        sharePolicy.GetMember(user.TrollId).ReturnsNull();

        sharesRepository.GetSharePolicyAsync(sharePolicyId, cancellationToken).Returns(sharePolicy);
        var sharesService = new SharesService(sharesRepository, Substitute.For<ISharesAcl>());

        // Act
        var result = await sharesService.GetSharePolicyAsync(user, sharePolicyId, cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]

    public async Task GetSharePolicyAsync_WhenUserIsOnlyGuest_ReturnsNulls()
    {
        // Arrange
        var sharesRepository = Substitute.For<ISharesRepository>();
        var sharePolicyId = Guid.NewGuid();
        var cancellationToken = new CancellationToken();
        var user = new AppUser(Guid.NewGuid(), 1, "accountId");
        var sharePolicy = Substitute.For<ISharePolicy>();
        sharePolicy.Id.Returns(sharePolicyId);
        sharePolicy.Name.Returns("Name");
        var member = Substitute.For<IMember>();
        member.IsGuest.Returns(true);
        sharePolicy.GetMember(user.TrollId).Returns(member);

        sharesRepository.GetSharePolicyAsync(sharePolicyId, cancellationToken).Returns(sharePolicy);
        var sharesService = new SharesService(sharesRepository, Substitute.For<ISharesAcl>());

        // Act
        var result = await sharesService.GetSharePolicyAsync(user, sharePolicyId, cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    private static ISharePolicy ToDomain(SharePolicyDto sharePolicyDto)
    {
        var expectedSharePolicy = Substitute.For<ISharePolicy>();
        expectedSharePolicy.Id.Returns(sharePolicyDto.Id);
        expectedSharePolicy.Name.Returns(sharePolicyDto.Name);
        var expectedMembers = sharePolicyDto.Members.Select(ToDomain).ToArray();
        expectedSharePolicy.Members.Returns(expectedMembers);
        return expectedSharePolicy;
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