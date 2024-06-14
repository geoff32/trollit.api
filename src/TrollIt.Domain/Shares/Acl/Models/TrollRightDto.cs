namespace TrollIt.Domain.Shares.Acl.Models;

public record TrollRightDto(int TrollId, IEnumerable<FeatureDto> Features);