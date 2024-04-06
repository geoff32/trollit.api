using Xunit;
using FluentAssertions;
using TrollIt.Domain.Profiles;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Domain.Tests.Profiles;

public class ProfileTests
{
    [Fact]
    public void Profile_CreatesCorrectlyFromProfileDto()
    {
        // Arrange
        var bonusMalusDto = new BonusMalusDto(Physical: 2, Magical: 1);
        var attackDto = new DiceAttributeDto(Value: 10, BonusMalus: bonusMalusDto);
        var dodgeDto = new DiceAttributeDto(Value: 10, BonusMalus: bonusMalusDto);
        var damageDto = new DiceAttributeDto(Value: 10, BonusMalus: bonusMalusDto);
        var armorDto = new DiceAttributeDto(Value: 10, BonusMalus: bonusMalusDto);
        var regenerationDto = new DiceAttributeDto(Value: 10, BonusMalus: bonusMalusDto);

        var profileDto = new ProfileDto(
            TrollId: 1,
            TurnDuration: new ValueAttributeDto<TimeSpan>(TimeSpan.FromHours(10), new BonusMalusDto<TimeSpan>(TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(40))),
            Vitality: new ValueAttributeDto(150, new BonusMalusDto(10, 5)),
            View: new ValueAttributeDto(10, new BonusMalusDto(2, 1)),
            Attack: attackDto,
            Dodge: dodgeDto,
            Damage: damageDto,
            Armor: armorDto,
            Regeneration: regenerationDto,
            MagicMastery: new ValueAttributeDto(1000, new BonusMalusDto(400, 50)),
            MagicResistance: new ValueAttributeDto(1000, new BonusMalusDto(400, 50))
        );

        // Act
        var profile = new Profile(profileDto);

        // Assert
        profile.TrollId.Should().Be(profileDto.TrollId);
        profile.TurnDuration.Value.Should().Be(profileDto.TurnDuration.Value);
        profile.TurnDuration.BonusMalus.Physical.Should().Be(profileDto.TurnDuration.BonusMalus.Physical);
        profile.TurnDuration.BonusMalus.Magical.Should().Be(profileDto.TurnDuration.BonusMalus.Magical);
        profile.Vitality.Value.Should().Be(profileDto.Vitality.Value);
        profile.Vitality.BonusMalus.Physical.Should().Be(profileDto.Vitality.BonusMalus.Physical);
        profile.Vitality.BonusMalus.Magical.Should().Be(profileDto.Vitality.BonusMalus.Magical);
        profile.View.Value.Should().Be(profileDto.View.Value);
        profile.View.BonusMalus.Physical.Should().Be(profileDto.View.BonusMalus.Physical);
        profile.View.BonusMalus.Magical.Should().Be(profileDto.View.BonusMalus.Magical);
        profile.Attack.Value.Should().Be(profileDto.Attack.Value);
        profile.Attack.BonusMalus.Physical.Should().Be(profileDto.Attack.BonusMalus.Physical);
        profile.Attack.BonusMalus.Magical.Should().Be(profileDto.Attack.BonusMalus.Magical);
        profile.Dodge.Value.Should().Be(profileDto.Dodge.Value);
        profile.Dodge.BonusMalus.Physical.Should().Be(profileDto.Dodge.BonusMalus.Physical);
        profile.Dodge.BonusMalus.Magical.Should().Be(profileDto.Dodge.BonusMalus.Magical);
        profile.Damage.Value.Should().Be(profileDto.Damage.Value);
        profile.Damage.BonusMalus.Physical.Should().Be(profileDto.Damage.BonusMalus.Physical);
        profile.Damage.BonusMalus.Magical.Should().Be(profileDto.Damage.BonusMalus.Magical);
        profile.Armor.Value.Should().Be(profileDto.Armor.Value);
        profile.Armor.BonusMalus.Physical.Should().Be(profileDto.Armor.BonusMalus.Physical);
        profile.Armor.BonusMalus.Magical.Should().Be(profileDto.Armor.BonusMalus.Magical);
        profile.Regeneration.Value.Should().Be(profileDto.Regeneration.Value);
        profile.Regeneration.BonusMalus.Physical.Should().Be(profileDto.Regeneration.BonusMalus.Physical);
        profile.Regeneration.BonusMalus.Magical.Should().Be(profileDto.Regeneration.BonusMalus.Magical);
        profile.MagicMastery.BonusMalus.Physical.Should().Be(profileDto.MagicMastery.BonusMalus.Physical);
        profile.MagicMastery.BonusMalus.Magical.Should().Be(profileDto.MagicMastery.BonusMalus.Magical);
        profile.MagicMastery.Value.Should().Be(profileDto.MagicMastery.Value);
        profile.MagicResistance.Value.Should().Be(profileDto.MagicResistance.Value);
        profile.MagicResistance.BonusMalus.Physical.Should().Be(profileDto.MagicResistance.BonusMalus.Physical);
        profile.MagicResistance.BonusMalus.Magical.Should().Be(profileDto.MagicResistance.BonusMalus.Magical);
    }
}