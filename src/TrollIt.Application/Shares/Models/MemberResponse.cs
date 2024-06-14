using TrollIt.Domain.Shares.Abstractions;

namespace TrollIt.Application.Shares.Models;

public record MemberResponse(int Id, RightResponse Profile, RightResponse View)
{
    public MemberResponse(IMember member)
        : this(member.Id, new RightResponse(member.Features.First(f => f.Id == FeatureId.Profile)), new RightResponse(member.Features.First(f => f.Id == FeatureId.View)))
    {
        
    }
}