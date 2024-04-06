using FluentAssertions;
using TrollIt.Domain.Profiles;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Domain.Tests.Profiles;

public class DamageTests
{
    [Fact]
    public void Damage_CreatesCorrectlyFromDiceAttributeDto()
    {
        // Arrange
        var bonusMalusDto = new BonusMalusDto(Physical: 2, Magical: 1);
        var diceAttributeDto = new DiceAttributeDto(Value: 10, BonusMalus: bonusMalusDto);

        // Act
        var damage = new Damage(diceAttributeDto);

        // Assert
        damage.Dice.Side.Should().Be(3);
        damage.Value.Should().Be(diceAttributeDto.Value);
        damage.BonusMalus.Physical.Should().Be(diceAttributeDto.BonusMalus.Physical);
        damage.BonusMalus.Magical.Should().Be(diceAttributeDto.BonusMalus.Magical);
    }
}