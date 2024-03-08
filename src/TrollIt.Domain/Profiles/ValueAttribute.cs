using TrollIt.Domain.Profiles.Abstractions;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Domain.Profiles;

internal record ValueAttribute(int Value, IBonusMalus BonusMalus) : IValueAttribute
{
    public ValueAttribute(ValueAttributeDto valueAttributeDto)
        : this(valueAttributeDto.Value, new BonusMalus(valueAttributeDto.BonusMalus))
    {
    }
}
