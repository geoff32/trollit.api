using TrollIt.Domain.Accounts.Abstractions;

namespace TrollIt.Api.Authorization;

public record TrollResource(int TrollId, FeatureId FeatureId);