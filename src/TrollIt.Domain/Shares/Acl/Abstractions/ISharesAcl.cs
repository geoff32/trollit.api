using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;

namespace TrollIt.Domain.Shares.Acl.Abstractions
{
    public interface ISharesAcl
    {
        IPolicy ToDomain(PolicyDto policyDto);
        IInvitation ToDomain(InvitationDto invitationDto);
    }
}