using System.Text.Json.Serialization;
using TrollIt.Domain.Accounts.Abstractions;

namespace TrollIt.Application.Accounts.Models;

[method:JsonConstructor]

public record TrollResponse(int Id, string Name)
{
    public TrollResponse(ITroll troll)
        : this(troll.Id, troll.Name)
    {
    }
}
