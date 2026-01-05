using TrollIt.Domain.Accounts.Abstractions;

namespace TrollIt.Domain.Accounts.Acl.Models;
public record FeatureDto(FeatureId Id, bool CanRead, bool CanRefresh);