using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using TrollIt.Domain.Shares.Infrastructure;

namespace TrollIt.Api.Authorization;

public class TrollResourceAuthorizationHandler(ISharesRepository sharesRepository) : AuthorizationHandler<OperationAuthorizationRequirement, TrollResource>
{
    private readonly ISharesRepository _sharesRepository = sharesRepository;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        OperationAuthorizationRequirement requirement, TrollResource trollResource)
    {
        var trollId = context.User.GetAppUserFromClaims().TrollId;

        var cancellationToken = new CancellationTokenSource().Token;
        var userPolicy =
            await _sharesRepository.GetUserPolicyAsync(trollId, cancellationToken);
        
        if (requirement.Name == TrollOperations.Read.Name
            && !userPolicy.CanRead(trollResource.FeatureId, trollResource.TrollId))
        {
            context.Fail(new AuthorizationFailureReason(this, "User does not have permission to read this resource"));
        }
        
        if (requirement.Name == TrollOperations.Refresh.Name
            && !userPolicy.CanRefresh(trollResource.FeatureId, trollResource.TrollId))
        {
            context.Fail(new AuthorizationFailureReason(this, "User does not have permission to refresh this resource"));
        }
        
        context.Succeed(requirement);
    }
}