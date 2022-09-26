using System.Security.Claims;
using IdentityModel;

namespace Infinite.Client.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirstValue(JwtClaimTypes.Email);

    public static string GetUserName(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirstValue(JwtClaimTypes.Name);

    public static string GetPhoneNumber(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirstValue(JwtClaimTypes.PhoneNumber);

    public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirstValue(JwtClaimTypes.Subject);
}