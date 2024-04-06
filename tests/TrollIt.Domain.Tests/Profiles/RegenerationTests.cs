using FluentAssertions;
using TrollIt.Domain.Profiles;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Domain.Tests.Profiles;

public class RegenerationTests
{
    [Fact]
    public void Regeneration_CreatesCorrectlyFromDiceAttributeDto()
    {
        // Arrange
        var bonusMalusDto = new BonusMalusDto(Physical: 2, Magical: 1);
        var diceAttributeDto = new DiceAttributeDto(Value: 10, BonusMalus: bonusMalusDto);

        // Act
        var regeneration = new Regeneration(diceAttributeDto);

        // Assert
        regeneration.Dice.Side.Should().Be(3);
        regeneration.Value.Should().Be(diceAttributeDto.Value);
        regeneration.BonusMalus.Physical.Should().Be(bonusMalusDto.Physical);
        regeneration.BonusMalus.Magical.Should().Be(bonusMalusDto.Magical);
    }
}