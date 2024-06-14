namespace TrollIt.Domain.Shares.Exceptions;

public enum SharesExceptions
{
    MemberNotFound,
    InvitationNotFound,
    InvitationAlreadyExistsInSharePolicy,
    IsAlreadyMember,
    NoReadAccess,
    NoRefreshAccess
}
