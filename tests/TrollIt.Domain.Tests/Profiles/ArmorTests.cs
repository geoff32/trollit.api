using FluentAssertions;
using TrollIt.Domain.Profiles;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Domain.Tests.Profiles;

public class ArmorTests
{
    [Fact]
    public void Armor_CreatesCorrectlyFromDiceAttributeDto()
    {
        // Arrange
        var bonusMalusDto = new BonusMalusDto(Physical: 2, Magical: 1);
        var diceAttributeDto = new DiceAttributeDto(Value: 10, BonusMalus: bonusMalusDto);

        // Act
        var armor = new Armor(diceAttributeDto);

        // Assert
        armor.Dice.Side.Should().Be(3);
        armor.Value.Should().Be(diceAttributeDto.Value);
        armor.BonusMalus.Physical.Should().Be(diceAttributeDto.BonusMalus.Physical);
        armor.BonusMalus.Magical.Should().Be(diceAttributeDto.BonusMalus.Magical);
    }
}