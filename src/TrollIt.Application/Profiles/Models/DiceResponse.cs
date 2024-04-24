using TrollIt.Domain.Profiles.Abstractions;

namespace TrollIt.Application.Profiles.Models;

public record DiceResponse(int Side)
{
    public DiceResponse(IDice dice) : this(dice.Side)
    {
    }
}