using FluentAssertions;
using TrollIt.Domain.Shares.Acl.Models;
using TrollIt.Domain.Shares;
using TrollIt.Domain.Shares.Abstractions;

namespace TrollIt.Domain.Tests.Shares;

public class TrollRightTests
{
    [Fact]
    public void Constructor_ShouldSetProperties_WhenCalledWithTrollRightDto()
    {
        // Arrange
        var trollRightDto = new TrollRightDto(1, [new FeatureDto(FeatureId.Profile, false, false)]);

        // Act
        var trollRight = new TrollRight(trollRightDto);

        // Assert
        trollRight.TrollId.Should().Be(trollRightDto.TrollId);
        trollRight.Features.Should().ContainSingle().Which.Should().BeEquivalentTo(new Feature(FeatureId.Profile, false, false));
    }
}