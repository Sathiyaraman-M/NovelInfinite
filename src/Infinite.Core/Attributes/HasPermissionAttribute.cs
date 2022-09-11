using Infinite.Shared.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Infinite.Core.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(AppPermissions appPermissions) : base(appPermissions.ToString())
    {
        
    }
}