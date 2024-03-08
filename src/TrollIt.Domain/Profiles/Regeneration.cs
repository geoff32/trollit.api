using TrollIt.Domain.Profiles.Abstractions;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Domain.Profiles;

internal record Regeneration(int Value, IBonusMalus BonusMalus) : DiceAttribute(new Dice(3), Value, BonusMalus)
{
    public Regeneration(DiceAttributeDto diceAttributeDto)
        : this(diceAttributeDto.Value, new BonusMalus(diceAttributeDto.BonusMalus))
    {        
    }
}
