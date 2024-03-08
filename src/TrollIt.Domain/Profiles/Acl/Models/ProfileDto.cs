namespace TrollIt.Domain.Profiles.Acl.Models;

public record ProfileDto
(
    int TrollId,
    ValueAttributeDto<TimeSpan> TurnDuration,
    ValueAttributeDto Vitality,
    ValueAttributeDto View,
    DiceAttributeDto Attack,
    DiceAttributeDto Dodge,
    DiceAttributeDto Damage,
    DiceAttributeDto Armor,
    DiceAttributeDto Regeneration,
    ValueAttributeDto MagicMastery,
    ValueAttributeDto MagicResistance
);

