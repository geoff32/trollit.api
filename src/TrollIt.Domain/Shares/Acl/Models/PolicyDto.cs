namespace TrollIt.Domain.Shares.Acl.Models;

public record PolicyDto(Guid Id, string Name, IEnumerable<MemberDto> Members);