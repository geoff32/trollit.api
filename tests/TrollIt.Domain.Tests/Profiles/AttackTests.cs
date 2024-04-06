using Xunit;
using FluentAssertions;
using TrollIt.Domain.Profiles;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Domain.Tests.Profiles;

public class AttackTests
{
    [Fact]
    public void Attack_CreatesCorrectlyFromDiceAttributeDto()
    {
        // Arrange
        var bonusMalusDto = new BonusMalusDto(Physical: 2, Magical: 1);
        var diceAttributeDto = new DiceAttributeDto(Value: 10, BonusMalus: bonusMalusDto);

        // Act
        var attack = new Attack(diceAttributeDto);

        // Assert
        attack.Dice.Side.Should().Be(6);
        attack.Value.Should().Be(diceAttributeDto.Value);
        attack.BonusMalus.Physical.Should().Be(diceAttributeDto.BonusMalus.Physical);
        attack.BonusMalus.Magical.Should().Be(diceAttributeDto.BonusMalus.Magical);
    }
}