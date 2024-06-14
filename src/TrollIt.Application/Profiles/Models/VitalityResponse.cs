using TrollIt.Domain.Profiles.Abstractions;

namespace TrollIt.Application.Profiles.Models;

public record VitalityResponse(int Max, int Value, BonusMalusResponse BonusMalusResponse)
{
    public VitalityResponse(IVitality vitality) : this(vitality.Max, vitality.Value, new BonusMalusResponse(vitality.BonusMalus))
    {
    }
}
