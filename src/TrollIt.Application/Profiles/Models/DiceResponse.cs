using System.Text.Json.Serialization;
using TrollIt.Domain.Profiles.Abstractions;

namespace TrollIt.Application.Profiles.Models;

[method:JsonConstructor]
public record DiceResponse(int Side)
{
    public DiceResponse(IDice dice) : this(dice.Side)
    {
    }
}