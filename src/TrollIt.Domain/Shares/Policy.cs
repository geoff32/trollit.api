using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;
using TrollIt.Domain.Shares.Exceptions;

namespace TrollIt.Domain.Shares
{
    internal class Policy(Guid id, string name, IEnumerable<IMember> members) : IPolicy
    {
        public Guid Id { get; } = id;
        public string Name { get; } = name;

        private readonly List<IMember> _members = members.ToList();
        public IReadOnlyCollection<IMember> Members => _members.AsReadOnly();

        public Policy(PolicyDto policyDto)
            : this(policyDto.Id, policyDto.Name, policyDto.Members.Select(member => new Member(member)))
        {
        }

        public void AddInvitation(IInvitation invitation)
        {
            if (invitation.PolicyId != Id)
            {
                throw new DomainException<SharesExceptions>(SharesExceptions.InvitationPolicyMismatch);
            }
            
            var member = GetMember(invitation.TrollId);
            if (member != null)
            {
                throw new DomainException<SharesExceptions>(
                    member.IsGuest
                        ? SharesExceptions.InvitationAlreadyExistsInPolicy
                        : SharesExceptions.IsAlreadyMember);
            }

            _members.Add(new Member(invitation.TrollId, PolicyStatus.Guest, invitation.Features));
        }

        public void AcceptInvitation(IInvitation invitation)
        {
            if (invitation.PolicyId != Id)
            {
                throw new DomainException<SharesExceptions>(SharesExceptions.InvitationPolicyMismatch);
            }
            
            RemoveInvitation(invitation);

            _members.Add(new Member(invitation.TrollId, PolicyStatus.User, invitation.Features));
        }

        public IInvitation GetInvitation(int trollId)
        {
            var member = GetMember(trollId)
                ?? throw new DomainException<SharesExceptions>(SharesExceptions.InvitationNotFound);

            if (!member.IsGuest)
            {
                throw new DomainException<SharesExceptions>(SharesExceptions.IsAlreadyMember);
            }
            
            return new Invitation(Id, trollId, [.. member.Features]);
        }

        public void RemoveInvitation(IInvitation invitation)
        {
            if (invitation.PolicyId != Id)
            {
                throw new DomainException<SharesExceptions>(SharesExceptions.InvitationPolicyMismatch);
            }
            
            var member = GetMember(invitation.TrollId)
                ?? throw new DomainException<SharesExceptions>(SharesExceptions.InvitationNotFound);

            if (!member.IsGuest)
            {
                throw new DomainException<SharesExceptions>(SharesExceptions.IsAlreadyMember);
            }

            _members.Remove(member);
        }

        public void RemoveMember(int memberId)
        {
            var member = GetMember(memberId)
                ?? throw new DomainException<SharesExceptions>(SharesExceptions.MemberNotFound);

            if (member.IsGuest)
            {
                throw new DomainException<SharesExceptions>(SharesExceptions.MemberNotFound);
            }

            _members.Remove(member);
        }

        public IMember? GetMember(int trollId) => _members.Find(member => member.Id == trollId);
    }
}