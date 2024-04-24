namespace TrollIt.Domain.Shares.Abstractions;

public interface ISharePolicy
{
    Guid Id { get; }
    string Name { get; }
    IEnumerable<IMember> Members { get; }

    void AddInvitation(int memberId);
    void AcceptInvitation(int memberId);
    void RemoveInvitation(int memberId);
    void RemoveMember(int memberId);
    IMember? GetMember(int trollId);
}
