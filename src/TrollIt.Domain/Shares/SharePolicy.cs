using TrollIt.Domain.Shares.Abstractions;
using TrollIt.Domain.Shares.Acl.Models;
using TrollIt.Domain.Shares.Exceptions;

namespace TrollIt.Domain.Shares
{
    internal class SharePolicy(Guid id, string name, IEnumerable<IMember> members) : ISharePolicy
    {
        public Guid Id { get; } = id;
        public string Name { get; } = name;

        private readonly List<IMember> _members = members.ToList();
        public IEnumerable<IMember> Members => _members.AsReadOnly();

        public SharePolicy(SharePolicyDto sharePolicyDto)
            : this(sharePolicyDto.Id, sharePolicyDto.Name, sharePolicyDto.Members.Select(member => new Member(member)))
        {
        }

        public void AddInvitation(int memberId)
        {
            var member = GetMember(memberId);
            if (member != null)
            {
                throw new DomainException<SharesExceptions>(
                    member.IsGuest
                        ? SharesExceptions.InvitationAlreadyExistsInSharePolicy
                        : SharesExceptions.IsAlreadyMember);
            }

            _members.Add(new Member(memberId, ShareStatus.Guest, []));
        }

        public void AcceptInvitation(int memberId)
        {
            RemoveInvitation(memberId);

            _members.Add(new Member(memberId, ShareStatus.User, []));
        }

        public void RemoveInvitation(int memberId)
        {
            var member = GetMember(memberId)
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