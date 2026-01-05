using System.Text.Json.Serialization;
using TrollIt.Domain.Profiles.Abstractions;

namespace TrollIt.Application.Profiles.Models;

[method:JsonConstructor]
public record TurnDurationResponse(TimeSpan Value, BonusMalusResponse<TimeSpan> BonusMalus)
{
    public TurnDurationResponse(ITurnDuration turnDuration) : this(turnDuration.Value, new BonusMalusResponse<TimeSpan>(turnDuration.BonusMalus))
    {
    }
}

