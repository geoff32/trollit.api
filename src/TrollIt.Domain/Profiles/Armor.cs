using TrollIt.Domain.Profiles.Abstractions;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Domain.Profiles;

internal record Armor(int Value, IBonusMalus BonusMalus) : DiceAttribute(new Dice(3), Value, BonusMalus)
{
    public Armor(DiceAttributeDto diceAttributeDto)
        : this(diceAttributeDto.Value, new BonusMalus(diceAttributeDto.BonusMalus))
    {        
    }
}
