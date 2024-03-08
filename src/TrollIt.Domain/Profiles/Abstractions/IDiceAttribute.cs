namespace TrollIt.Domain.Profiles.Abstractions;

public interface IDiceAttribute
{
    int Value { get; }
    IDice Dice { get; }
    IBonusMalus BonusMalus { get; }
}
