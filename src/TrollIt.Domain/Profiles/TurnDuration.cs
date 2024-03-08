using TrollIt.Domain.Profiles.Abstractions;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Domain.Profiles;

internal record TurnDuration(TimeSpan Value, IBonusMalus<TimeSpan> BonusMalus) : ITurnDuration
{
    public TurnDuration(ValueAttributeDto<TimeSpan> turnDuration)
        : this(turnDuration.Value, new BonusMalus<TimeSpan>(turnDuration.BonusMalus))
    {
    }
}
