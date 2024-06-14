using TrollIt.Domain.Shares.Abstractions;

namespace TrollIt.Application.Shares.Models;

public record SharePolicyResponse(Guid Id, string Name, IEnumerable<MemberResponse> Members)
{
    public SharePolicyResponse(ISharePolicy sharePolicy)
        : this(sharePolicy.Id, sharePolicy.Name, sharePolicy.Members.Select(m => new MemberResponse(m)))
    {

    }
}
