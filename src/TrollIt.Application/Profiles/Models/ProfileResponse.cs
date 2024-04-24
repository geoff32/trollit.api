using TrollIt.Domain.Profiles.Abstractions;

namespace TrollIt.Application.Profiles.Models;

public record ProfileResponse(
    int TrollId,
    TurnDurationResponse TurnDuration,
    VitalityResponse Vitality,
    ValueAttributeResponse View,
    DiceAttributeResponse Attack,
    DiceAttributeResponse Dodge,
    DiceAttributeResponse Damage,
    DiceAttributeResponse Armor,
    DiceAttributeResponse Regeneration,
    ValueAttributeResponse MagicMastery,
    ValueAttributeResponse MagicResistance
)
{
    public ProfileResponse(IProfile profile)
        : this
        (profile.TrollId,
         new TurnDurationResponse(profile.TurnDuration),
         new VitalityResponse(profile.Vitality),
         new ValueAttributeResponse(profile.View),
         new DiceAttributeResponse(profile.Attack),
         new DiceAttributeResponse(profile.Dodge),
         new DiceAttributeResponse(profile.Damage),
         new DiceAttributeResponse(profile.Armor),
         new DiceAttributeResponse(profile.Regeneration),
         new ValueAttributeResponse(profile.MagicMastery),
         new ValueAttributeResponse(profile.MagicResistance))
    {
    }
}
