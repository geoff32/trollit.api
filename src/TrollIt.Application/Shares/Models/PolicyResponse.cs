using TrollIt.Domain.Shares.Abstractions;

namespace TrollIt.Application.Shares.Models;

public record PolicyResponse(Guid Id, string Name, IEnumerable<MemberResponse> Members)
{
    public PolicyResponse(IPolicy policy)
        : this(policy.Id, policy.Name, policy.Members.Select(m => new MemberResponse(m)))
    {

    }
}
