using TrollIt.Domain.Profiles.Abstractions;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Domain.Profiles;

internal record BonusMalus(int Physical, int Magical) : BonusMalus<int>(Physical, Magical), IBonusMalus
{
    public BonusMalus(BonusMalusDto<int> bonusMalusDto)
        : this(bonusMalusDto.Physical, bonusMalusDto.Magical)
    {
    }
}

internal record BonusMalus<T>(T Physical, T Magical) : IBonusMalus<T>
{
    public BonusMalus(BonusMalusDto<T> bonusMalusDto)
        : this(bonusMalusDto.Physical, bonusMalusDto.Magical)
    {
    }
}