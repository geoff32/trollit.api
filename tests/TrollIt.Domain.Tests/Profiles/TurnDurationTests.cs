using FluentAssertions;
using TrollIt.Domain.Profiles;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Domain.Tests.Profiles;

public class TurnDurationTests
{
    [Fact]
    public void TurnDuration_CreatesCorrectlyFromValueAttributeDto()
    {
        // Arrange
        var bonusMalusDto = new BonusMalusDto<TimeSpan>(Physical: TimeSpan.FromMinutes(30), Magical: TimeSpan.FromMinutes(20));
        var valueAttributeDto = new ValueAttributeDto<TimeSpan>(Value: TimeSpan.FromMinutes(10), BonusMalus: bonusMalusDto);

        // Act
        var turnDuration = new TurnDuration(valueAttributeDto);

        // Assert
        turnDuration.Value.Should().Be(valueAttributeDto.Value);
        turnDuration.BonusMalus.Physical.Should().Be(bonusMalusDto.Physical);
        turnDuration.BonusMalus.Magical.Should().Be(bonusMalusDto.Magical);
    }
}