using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;

namespace TrollIt.Domain.Shares;

internal record Member(int Id, ShareStatus Status, IEnumerable<IFeature> Features) : IMember
{
    public Member(MemberDto memberDto)
        : this
        (
            memberDto.Id,
            memberDto.Status,
            memberDto.Features.Select(f => new Feature(f))
        )
    {
    }
}