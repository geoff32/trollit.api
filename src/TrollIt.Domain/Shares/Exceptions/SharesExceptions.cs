namespace TrollIt.Domain.Shares.Exceptions;

public enum SharesExceptions
{
    MemberNotFound,
    InvitationNotFound,
    InvitationAlreadyExistsInPolicy,
    IsAlreadyMember,
    NoReadAccess,
    NoWriteAccess
}
