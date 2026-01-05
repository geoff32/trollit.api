namespace TrollIt.Domain.Shares.Abstractions;

public interface IPolicy
{
    Guid Id { get; }
    string Name { get; }
    IReadOnlyCollection<IMember> Members { get; }

    void AddInvitation(IInvitation invitation);
    IInvitation GetInvitation(int trollId);
    void AcceptInvitation(IInvitation invitation);
    void RemoveInvitation(IInvitation invitation);
    void RemoveMember(int memberId);
    IMember? GetMember(int trollId);
}
