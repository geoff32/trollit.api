using TrollIt.Domain.Profiles.Abstractions;

namespace TrollIt.Application.Profiles.Models;

public record DiceAttributeResponse(int Value, DiceResponse DiceSide, BonusMalusResponse BonusMalus)
{
    public DiceAttributeResponse(IDiceAttribute diceAttribute)
        : this(diceAttribute.Value, new DiceResponse(diceAttribute.Dice), new BonusMalusResponse(diceAttribute.BonusMalus))
    {
    }
}
