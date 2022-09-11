using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Infinite.Core.Permissions;

public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private readonly AuthorizationOptions _options;
    public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
        _options = options.Value;
    }

    public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        return await base.GetPolicyAsync(policyName) ??
               new AuthorizationPolicyBuilder()
                   .AddRequirements(new PermissionRequirement(policyName))
                   .Build();
    }
}