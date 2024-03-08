namespace TrollIt.Domain.Profiles.Abstractions;

public interface IProfile
{
    int TrollId { get; }
    ITurnDuration TurnDuration { get; }
    IVitality Vitality { get; }
    IValueAttribute View { get; }
    IDiceAttribute Attack { get; }
    IDiceAttribute Dodge { get; }
    IDiceAttribute Damage { get; }
    IDiceAttribute Armor { get; }
    IDiceAttribute Regeneration { get; }
    IValueAttribute MagicMastery { get; }
    IValueAttribute MagicResistance { get; }
}
