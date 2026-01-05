namespace TrollIt.Domain.Accounts.Acl.Models;

public record TrollRightDto(int TrollId, IEnumerable<FeatureDto> Features);