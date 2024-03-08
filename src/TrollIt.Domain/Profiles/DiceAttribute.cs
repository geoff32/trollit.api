namespace TrollIt.Domain.Profiles.Abstractions;

internal abstract record DiceAttribute(IDice Dice, int Value, IBonusMalus BonusMalus) : IDiceAttribute;