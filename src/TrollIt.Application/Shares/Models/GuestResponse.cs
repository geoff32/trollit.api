using TrollIt.Domain.Shares.Abstractions;

namespace TrollIt.Application.Shares.Models;

public record GuestResponse(int Id )
{
    public GuestResponse(IMember guest)
        : this(guest.Id)
    {
    }
}