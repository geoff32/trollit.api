using FluentAssertions;
using TrollIt.Domain.Shares;
using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;

namespace TrollIt.Domain.Tests.Shares;

public class FeatureTests
{
    [Fact]
    public void Constructor_ShouldSetProperties_WhenCalledWithFeatureDto()
    {
        // Arrange
        var featureDto = new FeatureDto(FeatureId.Profile, true, false);

        // Act
        var feature = new Feature(featureDto);

        // Assert
        feature.Id.Should().Be(featureDto.Id);
        feature.CanRead.Should().Be(featureDto.CanRead);
        feature.CanRefresh.Should().Be(featureDto.CanRefresh);
    }
}