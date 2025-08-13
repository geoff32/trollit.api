using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using TrollIt.Domain.Accounts.Infrastructure;

namespace TrollIt.Api.Authorization;

public class TrollResourceAuthorizationHandler(IAccountsRepository accountsRepository) : AuthorizationHandler<OperationAuthorizationRequirement, TrollResource>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        OperationAuthorizationRequirement requirement, TrollResource trollResource)
    {
        var trollId = context.User.GetAppUserFromClaims().TrollId;

        var cancellationToken = new CancellationTokenSource().Token;
        var accountPolicy =
            await accountsRepository.GetAccountPoliciesAsync(trollId, cancellationToken);
        
        if (requirement.Name == TrollOperations.Read.Name
            && !accountPolicy.CanRead(trollResource.FeatureId, trollResource.TrollId))
        {
            context.Fail(new AuthorizationFailureReason(this, "User does not have permission to read this resource"));
        }
        
        if (requirement.Name == TrollOperations.Refresh.Name
            && !accountPolicy.CanRefresh(trollResource.FeatureId, trollResource.TrollId))
        {
            context.Fail(new AuthorizationFailureReason(this, "User does not have permission to refresh this resource"));
        }
        
        context.Succeed(requirement);
    }
}