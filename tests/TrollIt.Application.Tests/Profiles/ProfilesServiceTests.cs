using NSubstitute;
using FluentAssertions;
using TrollIt.Domain.Profiles.Infrastructure;
using TrollIt.Domain.Shares.Infrastructure;
using TrollIt.Domain.Accounts.Infrastructure;
using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Profiles.Abstractions;
using TrollIt.Application.Profiles.Models;
using TrollIt.Domain.Accounts.Abstractions;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Application.Profiles.Tests;
public class ProfilesServiceTests
{
    [Fact]
    public async Task GetProfileAsync_ShouldReturnProfileResponse_WhenProfileExists()
    {
        // Arrange
        var profilesRepository = Substitute.For<IProfilesRepository>();
        var sharesRepository = Substitute.For<ISharesRepository>();
        var accountsRepository = Substitute.For<IAccountsRepository>();
        var profilesService = new ProfilesService(profilesRepository, sharesRepository, accountsRepository);
        var user = new AppUser(Guid.NewGuid(), 1, "accountId");
        var trollId = 1;
        var cancellationToken = CancellationToken.None;
        var userPolicy = Substitute.For<IUserPolicy>();
        var profileDto = GetProfileDto(trollId);
        sharesRepository.GetUserPolicyAsync(user.TrollId, cancellationToken).Returns(userPolicy);
        var profile = Mock(profileDto);
        profilesRepository.GetProfileAsync(trollId, cancellationToken).Returns(profile);

        // Act
        var result = await profilesService.GetProfileAsync(user, trollId, cancellationToken);

        // Assert
        result.Should().NotBeNull()
            .And.BeOfType<ProfileResponse>()
            .And.BeEquivalentTo(ToResponse(profileDto));
    }

    [Fact]
    public async Task RefreshProfileAsync_ShouldReturnProfileResponse_WhenAccountExists()
    {
        // Arrange
        var profilesRepository = Substitute.For<IProfilesRepository>();
        var sharesRepository = Substitute.For<ISharesRepository>();
        var accountsRepository = Substitute.For<IAccountsRepository>();
        var profilesService = new ProfilesService(profilesRepository, sharesRepository, accountsRepository);
        var user = new AppUser(Guid.NewGuid(), 1, "accountId");
        var trollId = 1;
        var cancellationToken = CancellationToken.None;
        var userPolicy = Substitute.For<IUserPolicy>();
        var account = Substitute.For<IAccount>();
        var profileDto = GetProfileDto(trollId);
        var profile = Mock(profileDto);
        
        sharesRepository.GetUserPolicyAsync(user.TrollId, cancellationToken).Returns(userPolicy);
        accountsRepository.GetAccountByTrollAsync(trollId, cancellationToken).Returns(account);
        profilesRepository.RefreshProfileAsync(trollId, Arg.Any<string>(), cancellationToken).Returns(profile);

        // Act
        var result = await profilesService.RefreshProfileAsync(user, trollId, cancellationToken);

        // Assert
        result.Should().NotBeNull()
            .And.BeOfType<ProfileResponse>()
            .And.BeEquivalentTo(ToResponse(profileDto));
    }

    private static ProfileDto GetProfileDto(int trollId)
    {
        return new ProfileDto
        (
            TrollId: trollId,
            TurnDuration: new ValueAttributeDto<TimeSpan>(TimeSpan.FromHours(10), new BonusMalusDto<TimeSpan>(TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(-20))),
            Vitality: new ValueAttributeDto(150, new BonusMalusDto(0, 10)),
            View: new ValueAttributeDto(3, new BonusMalusDto(-2, 5)),
            Attack: new DiceAttributeDto(7, new BonusMalusDto(1, 6)),
            Dodge: new DiceAttributeDto(9, new BonusMalusDto(5, 4)),
            Damage: new DiceAttributeDto(7, new BonusMalusDto(1, 6)),
            Armor: new DiceAttributeDto(7, new BonusMalusDto(1, 6)),
            Regeneration: new DiceAttributeDto(7, new BonusMalusDto(1, 6)),
            MagicMastery: new ValueAttributeDto(4000, new BonusMalusDto(1600, 0)),
            MagicResistance: new ValueAttributeDto(1000, new BonusMalusDto(2600, 0))
        );
    }

    private static ProfileResponse ToResponse(ProfileDto profileDto) =>
        new(
            TrollId: profileDto.TrollId,
            TurnDuration: ToTurnDurationResponse(profileDto.TurnDuration),
            Vitality: ToVitalityResponse(profileDto.Vitality),
            View: ToResponse(profileDto.View),
            Attack: ToResponse(profileDto.Attack, 6),
            Dodge: ToResponse(profileDto.Dodge, 6),
            Damage: ToResponse(profileDto.Damage, 3),
            Armor: ToResponse(profileDto.Armor, 3),
            Regeneration: ToResponse(profileDto.Regeneration, 3),
            MagicMastery: ToResponse(profileDto.MagicMastery),
            MagicResistance: ToResponse(profileDto.MagicResistance)
        );

    private static DiceAttributeResponse ToResponse(DiceAttributeDto diceAttributeDto, int side) =>
        new(diceAttributeDto.Value, new DiceResponse(side), ToResponse(diceAttributeDto.BonusMalus));

    private static ValueAttributeResponse ToResponse(ValueAttributeDto valueAttributeDto) =>
        new(valueAttributeDto.Value, ToResponse(valueAttributeDto.BonusMalus));

    private static BonusMalusResponse ToResponse(BonusMalusDto bonusMalusDto) =>
        new(bonusMalusDto.Physical, bonusMalusDto.Magical);

    private static BonusMalusResponse<TimeSpan> ToResponse(BonusMalusDto<TimeSpan> bonusMalusDto) =>
        new(bonusMalusDto.Physical, bonusMalusDto.Magical);

    private static TurnDurationResponse ToTurnDurationResponse(ValueAttributeDto<TimeSpan> valueAttributeDto) =>
        new(valueAttributeDto.Value, ToResponse(valueAttributeDto.BonusMalus));

    private static VitalityResponse ToVitalityResponse(ValueAttributeDto vitalityDto) =>
        new
        (
            vitalityDto.Value + vitalityDto.BonusMalus.Physical + vitalityDto.BonusMalus.Magical, vitalityDto.Value,
            ToResponse(vitalityDto.BonusMalus)
        );

    private static IProfile Mock(ProfileDto profileDto)
    {
        var profile = Substitute.For<IProfile>();
        var turnDuration = Mock(profileDto.TurnDuration);
        var vitality = MockVitality(profileDto.Vitality);
        var mockView = Mock(profileDto.View);
        var mockAttack = Mock(profileDto.Attack, 6);
        var mockDodge = Mock(profileDto.Dodge, 6);
        var mockDamage = Mock(profileDto.Damage, 3);
        var mockArmor = Mock(profileDto.Armor, 3);
        var mockRegeneration = Mock(profileDto.Regeneration, 3);
        var mockMagicMastery = Mock(profileDto.MagicMastery);
        var mockMagicResistance = Mock(profileDto.MagicResistance);

        profile.TrollId.Returns(profileDto.TrollId);
        profile.TurnDuration.Returns(turnDuration);
        profile.Vitality.Returns(vitality);
        profile.View.Returns(mockView);
        profile.Attack.Returns(mockAttack);
        profile.Dodge.Returns(mockDodge);
        profile.Damage.Returns(mockDamage);
        profile.Armor.Returns(mockArmor);
        profile.Regeneration.Returns(mockRegeneration);
        profile.MagicMastery.Returns(mockMagicMastery);
        profile.MagicResistance.Returns(mockMagicResistance);

        return profile;
    }

    private static IDiceAttribute Mock(DiceAttributeDto diceAttributeDto, int diceSide)
    {
        var diceAttribute = Substitute.For<IDiceAttribute>();
        var bonusMalus = Substitute.For<IBonusMalus>();
        bonusMalus.Physical.Returns(diceAttributeDto.BonusMalus.Physical);
        bonusMalus.Magical.Returns(diceAttributeDto.BonusMalus.Magical);

        var dice = Substitute.For<IDice>();
        dice.Side.Returns(diceSide);

        diceAttribute.Value.Returns(diceAttributeDto.Value);
        diceAttribute.Dice.Returns(dice);
        diceAttribute.BonusMalus.Returns(bonusMalus);

        return diceAttribute;
    }

    private static IValueAttribute Mock(ValueAttributeDto valueAttributeDto)
    {
        var vitality = Substitute.For<IValueAttribute>();
        var bonusMalus = Substitute.For<IBonusMalus>();
        bonusMalus.Physical.Returns(valueAttributeDto.BonusMalus.Physical);
        bonusMalus.Magical.Returns(valueAttributeDto.BonusMalus.Magical);

        vitality.Value.Returns(valueAttributeDto.Value);
        vitality.BonusMalus.Returns(bonusMalus);

        return vitality;
    }

    private static ITurnDuration Mock(ValueAttributeDto<TimeSpan> turnDurationDto)
    {
        var turn = Substitute.For<ITurnDuration>();
        var bonusMalus = Substitute.For<IBonusMalus<TimeSpan>>();
        bonusMalus.Physical.Returns(turnDurationDto.BonusMalus.Physical);
        bonusMalus.Magical.Returns(turnDurationDto.BonusMalus.Magical);

        turn.Value.Returns(turnDurationDto.Value);
        turn.BonusMalus.Returns(bonusMalus);

        return turn;
    }

    private static IVitality MockVitality(ValueAttributeDto vitalityDto)
    {
        var vitality = Substitute.For<IVitality>();
        var bonusMalus = Substitute.For<IBonusMalus>();
        bonusMalus.Physical.Returns(vitalityDto.BonusMalus.Physical);
        bonusMalus.Magical.Returns(vitalityDto.BonusMalus.Magical);

        vitality.Max.Returns(vitalityDto.Value + vitalityDto.BonusMalus.Physical + vitalityDto.BonusMalus.Magical);
        vitality.Value.Returns(vitalityDto.Value);
        vitality.BonusMalus.Returns(bonusMalus);

        return vitality;
    }
}