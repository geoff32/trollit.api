using FluentAssertions;
using TrollIt.Domain.Profiles;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Domain.Tests.Profiles;

public class VitalityTests
{
    [Fact]
    public void Vitality_CreatesCorrectlyFromValueAttributeDto()
    {
        // Arrange
        var bonusMalusDto = new BonusMalusDto(Physical: 2, Magical: 1);
        var valueAttributeDto = new ValueAttributeDto(Value: 10, BonusMalus: bonusMalusDto);

        // Act
        var vitality = new Vitality(valueAttributeDto);

        // Assert
        vitality.Value.Should().Be(valueAttributeDto.Value);
        vitality.BonusMalus.Physical.Should().Be(bonusMalusDto.Physical);
        vitality.BonusMalus.Magical.Should().Be(bonusMalusDto.Magical);
    }

    [Fact]
    public void Vitality_Max_ReturnsReturnSumOfValueAndBonusMalus()
    {
        // Arrange
        var vitality = new Vitality(100, new BonusMalus(10, 5));

        // Act
        var max = vitality.Max;

        // Assert
        max.Should().Be(115);
    }
}