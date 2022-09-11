using System.Security.Claims;
using Infinite.Shared.Constants;

namespace Infinite.Core.Extensions;

public static class PermissionExtensions
{
    public static bool HasPermission(this ClaimsPrincipal user, AppPermissions permissionToCheck)
    {
        var packedPermissions = user?.Claims.SingleOrDefault(x => x.Type == ApplicationClaimTypes.Permission)?.Value;
        if (packedPermissions == null)
            return false;
        var permissionAsChar = (char)Convert.ChangeType(permissionToCheck, typeof(char));
        return packedPermissions.IsThisPermissionAllowed(permissionAsChar);
    }
    
    public static bool ThisPermissionIsAllowed(this string packedPermissions, string permissionName)
    {
        var permissionAsChar = (char)Convert.ChangeType(Enum.Parse<AppPermissions>(permissionName), typeof(char));
        return packedPermissions.IsThisPermissionAllowed(permissionAsChar);
    }
    
    private static bool IsThisPermissionAllowed(this string packedPermissions, char permissionAsChar)
    {
        return packedPermissions.Contains(permissionAsChar) ||
               packedPermissions.Contains(ApplicationClaimTypes.Permission);
    }
}