using TrollIt.Domain.Profiles.Abstractions;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Domain.Profiles;

internal record Attack(int Value, IBonusMalus BonusMalus) : DiceAttribute(new Dice(6), Value, BonusMalus)
{
    public Attack(DiceAttributeDto diceAttributeDto)
        : this(diceAttributeDto.Value, new BonusMalus(diceAttributeDto.BonusMalus))
    {        
    }
}

