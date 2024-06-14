using TrollIt.Domain.Profiles.Abstractions;

namespace TrollIt.Application.Profiles.Models;

public record ValueAttributeResponse(int Value, BonusMalusResponse BonusMalus)
{
    public ValueAttributeResponse(IValueAttribute valueAttribute) : this(valueAttribute.Value, new BonusMalusResponse(valueAttribute.BonusMalus))
    {
    }
}
