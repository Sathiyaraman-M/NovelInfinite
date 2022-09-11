using Infinite.Core.Extensions;
using Infinite.Shared.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Infinite.Core.Permissions;

public class PermissionPolicyHandler : AuthorizationHandler<PermissionRequirement>
{

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var permissionsClaim =
            context.User.Claims.SingleOrDefault(c => c.Type == ApplicationClaimTypes.Permission);
        // If user does not have the scope claim, get out of here
        if (permissionsClaim == null)
            return Task.CompletedTask;

        if (permissionsClaim.Value.ThisPermissionIsAllowed(requirement.PermissionName))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}