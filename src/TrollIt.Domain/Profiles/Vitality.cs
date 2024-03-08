using TrollIt.Domain.Profiles.Abstractions;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Domain.Profiles;

internal record Vitality(int Value, IBonusMalus BonusMalus) : IVitality
{
    public Vitality(ValueAttributeDto valueAttributeDto)
        : this(valueAttributeDto.Value, new BonusMalus(valueAttributeDto.BonusMalus))
    {
    }

    public int Max => Value + BonusMalus.Physical + BonusMalus.Magical;
}