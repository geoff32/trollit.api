namespace TrollIt.Domain.Accounts.Acl.Models;

public record AccountPolicyDto(int Id, IEnumerable<TrollRightDto> Rights);
