using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace TrollIt.Api.Authorization;

public static class TrollOperations
{
    public static readonly OperationAuthorizationRequirement Refresh = new() { Name = nameof(Refresh) };
    public static readonly OperationAuthorizationRequirement Read = new() { Name = nameof(Read) };
}