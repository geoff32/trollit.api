using TrollIt.Domain.Profiles.Abstractions;
using TrollIt.Domain.Profiles.Acl.Abstractions;
using TrollIt.Domain.Profiles.Acl.Models;
using TrollIt.Infrastructure.Mountyhall.Models.Profile;
using TrollIt.Infrastructure.Profiles.Acl.Abstractions;

namespace TrollIt.Infrastructure.Profiles.Acl;

internal class ProfilesRepositoryAcl(IProfilesAcl profilesAcl) : IProfilesRepositoryAcl
{
    public IProfile ToDomain(Profile profile)
    {
        var profileDto = new ProfileDto
        (
            TrollId: profile.Troll.Id,
            TurnDuration: GetDuration(profile.Caracs.Dla),
            Vitality: GetValueAttribute(profile.Caracs.PvMax),
            View: GetValueAttribute(profile.Caracs.Vue),
            Attack: GetDiceAttribute(profile.Caracs.Att),
            Dodge: GetDiceAttribute(profile.Caracs.Esq),
            Damage: GetDiceAttribute(profile.Caracs.Deg),
            Armor: GetDiceAttribute(profile.Caracs.Arm),
            Regeneration: GetDiceAttribute(profile.Caracs.Reg),
            MagicMastery: GetValueAttribute(profile.Caracs.Mm),
            MagicResistance: GetValueAttribute(profile.Caracs.Rm)
        );

        return profilesAcl.ToDomain(profileDto);
    }

    public IProfile? ToDomain(Models.Troll? troll)
    {
        return troll == null ? null
            : profilesAcl.ToDomain(new ProfileDto
            (
                TrollId: troll.Id,
                TurnDuration: GetDuration(troll.Profile.Turnduration),
                Vitality: GetValueAttribute(troll.Profile.Vitality),
                View: GetValueAttribute(troll.Profile.View),
                Attack: GetDiceAttribute(troll.Profile.Attack),
                Dodge: GetDiceAttribute(troll.Profile.Dodge),
                Damage: GetDiceAttribute(troll.Profile.Damage),
                Armor: GetDiceAttribute(troll.Profile.Armor),
                Regeneration: GetDiceAttribute(troll.Profile.Regeneration),
                MagicMastery: GetValueAttribute(troll.Profile.Magicmastery),
                MagicResistance: GetValueAttribute(troll.Profile.Magicresistance)
            ));
    }

    private static DiceAttributeDto GetDiceAttribute(Carac carac) =>
        new(Value: (int)carac.Car, BonusMalus: new BonusMalusDto(Physical: (int)carac.Bmp, Magical: (int)carac.Bmm));
        
    private static DiceAttributeDto GetDiceAttribute(Models.Attribute attribute) =>
        new
        (
            Value: attribute.Value,
            BonusMalus: new BonusMalusDto(Physical: attribute.Physicalbonus, Magical: attribute.Magicalbonus)
        );

    private static ValueAttributeDto GetValueAttribute(Carac carac) =>
        new(Value: (int)carac.Car, BonusMalus: new BonusMalusDto(Physical: (int)carac.Bmp, Magical: (int)carac.Bmm));

    private static ValueAttributeDto GetValueAttribute(Models.Attribute attribute) =>
        new
        (
            Value: attribute.Value,
            BonusMalus: new BonusMalusDto(Physical: attribute.Physicalbonus, Magical: attribute.Magicalbonus)
        );

    private static ValueAttributeDto<TimeSpan> GetDuration(Carac carac) =>
        new
        (
            Value: TimeSpan.FromMinutes((int)carac.Car),
            BonusMalus: new BonusMalusDto<TimeSpan>
            (
                Physical: TimeSpan.FromMinutes((int)carac.Bmp),
                Magical: TimeSpan.FromMinutes((int)carac.Bmm)
            )
        );

    private static ValueAttributeDto<TimeSpan> GetDuration(Models.Attribute attribute) =>
        new
        (
            Value: TimeSpan.FromMinutes(attribute.Value),
            BonusMalus: new BonusMalusDto<TimeSpan>
            (
                Physical: TimeSpan.FromMinutes(attribute.Physicalbonus),
                Magical: TimeSpan.FromMinutes(attribute.Magicalbonus)
            )
        );
}
