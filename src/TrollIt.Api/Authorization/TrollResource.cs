using TrollIt.Domain.Shares.Abstractions;

namespace TrollIt.Api.Authorization;

public record TrollResource(int TrollId, FeatureId FeatureId);