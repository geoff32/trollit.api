using System.Text.Json.Serialization;
using TrollIt.Domain.Profiles.Abstractions;

namespace TrollIt.Application.Profiles.Models;

[method:JsonConstructor]
public record BonusMalusResponse(int Physical, int Magical) : BonusMalusResponse<int>(Physical, Magical)
{
    public BonusMalusResponse(IBonusMalus bonusMalus) : this(bonusMalus.Physical, bonusMalus.Magical)
    {
    }
}

[method:JsonConstructor]
public record BonusMalusResponse<T>(T Physical, T Magical)
{
    public BonusMalusResponse(IBonusMalus<T> bonusMalus) : this(bonusMalus.Physical, bonusMalus.Magical)
    {
    }
}

