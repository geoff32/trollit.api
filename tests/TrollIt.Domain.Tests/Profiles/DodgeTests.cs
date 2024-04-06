using FluentAssertions;
using TrollIt.Domain.Profiles;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Domain.Tests.Profiles;

public class DodgeTests
{
    [Fact]
    public void Dodge_CreatesCorrectlyFromDiceAttributeDto()
    {
        // Arrange
        var bonusMalusDto = new BonusMalusDto(Physical: 2, Magical: 1);
        var diceAttributeDto = new DiceAttributeDto(Value: 10, BonusMalus: bonusMalusDto);

        // Act
        var dodge = new Dodge(diceAttributeDto);

        // Assert
        dodge.Dice.Side.Should().Be(6);
        dodge.Value.Should().Be(diceAttributeDto.Value);
        dodge.BonusMalus.Physical.Should().Be(diceAttributeDto.BonusMalus.Physical);
        dodge.BonusMalus.Magical.Should().Be(diceAttributeDto.BonusMalus.Magical);
    }
}