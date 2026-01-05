using TrollIt.Domain.Shares.Abstractions;

namespace TrollIt.Domain.Shares.Acl.Models;
public record FeatureDto(FeatureId Id, bool CanRead, bool CanRefresh);