namespace TrollIt.Domain.Shares.Acl.Models;

public record InvitationDto(Guid PolicyId, int TrollId, IReadOnlyCollection<FeatureDto> Features);