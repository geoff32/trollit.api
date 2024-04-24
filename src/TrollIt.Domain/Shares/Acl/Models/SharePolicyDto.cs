namespace TrollIt.Domain.Shares.Acl.Models;

public record SharePolicyDto(Guid Id, string Name, IEnumerable<MemberDto> Members);