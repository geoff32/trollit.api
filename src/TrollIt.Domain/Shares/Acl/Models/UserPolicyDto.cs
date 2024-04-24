namespace TrollIt.Domain.Shares.Acl.Models;

public record UserPolicyDto(int Id, IEnumerable<TrollRightDto> Rights);
