using TrollIt.Domain.Profiles.Abstractions;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Domain.Profiles;

internal record Profile
(
    int TrollId,
    ITurnDuration TurnDuration,
    IVitality Vitality,
    IValueAttribute View,
    IDiceAttribute Attack,
    IDiceAttribute Dodge,
    IDiceAttribute Damage,
    IDiceAttribute Armor,
    IDiceAttribute Regeneration,
    IValueAttribute MagicMastery,
    IValueAttribute MagicResistance
) : IProfile
{
    public Profile(ProfileDto profileDto)
        : this
        (
            profileDto.TrollId,
            new TurnDuration(profileDto.TurnDuration),
            new Vitality(profileDto.Vitality),
            new ValueAttribute(profileDto.View),
            new Attack(profileDto.Attack),
            new Dodge(profileDto.Dodge),
            new Damage(profileDto.Damage),
            new Armor(profileDto.Armor),
            new Regeneration(profileDto.Regeneration),
            new ValueAttribute(profileDto.MagicMastery),
            new ValueAttribute(profileDto.MagicResistance)
        )
    {
        
    }
}
