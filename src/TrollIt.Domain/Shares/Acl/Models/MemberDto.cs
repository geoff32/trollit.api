using TrollIt.Domain.Shares.Abstractions;

namespace TrollIt.Domain.Shares.Acl.Models;
public record MemberDto(int Id, ShareStatus Status, IEnumerable<FeatureDto> Features);