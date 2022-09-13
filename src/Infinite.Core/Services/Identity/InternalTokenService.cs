using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Infinite.Core.Interfaces.Services.Identity;
using Infinite.Shared.Configurations;
using Infinite.Shared.Constants;
using Infinite.Shared.Entities;
using Infinite.Shared.Requests.Identity;
using Infinite.Shared.Responses.Identity;
using Infinite.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infinite.Core.Services.Identity;

public class InternalTokenService : ITokenService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AppConfiguration _appConfiguration;

    public InternalTokenService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<AppConfiguration> options)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _appConfiguration = options.Value;
    }
    
    
    
    public async Task<IResult<TokenResponse>> LoginAsync(TokenRequest tokenRequest)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(tokenRequest.UserName);
            if (user == null)
            {
                throw new Exception("User Not Found");
            }
            if (user.IsDeleted)
            {
                throw new Exception("User Not Found.");
            }
            if (!user.IsActive)
            {
                throw new Exception("User Not Active. Please contact the support.");
            }
            if (!user.EmailConfirmed)
            {
                throw new Exception("E-Mail not confirmed.");
            }
            var passwordValid = await _userManager.CheckPasswordAsync(user, tokenRequest.Password);
            if (!passwordValid)
            {
                throw new Exception("Invalid Credentials.");
            }

            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);

            var token = await GenerateJwtAsync(user);
            var response = new TokenResponse
            {
                Token = token,
                RefreshToken = user.RefreshToken,
                UserImageUrl = user.ProfilePictureDataUrl,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime
            };
            return await Result<TokenResponse>.SuccessAsync(response);
        }
        catch (Exception e)
        {
            return await Result<TokenResponse>.FailAsync(e.Message);
        }
    }

    public async Task<IResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        try
        {
            if (refreshTokenRequest is null)
            {
                throw new Exception("Invalid Client Token.");
            }
            var userPrincipal = GetPrincipalFromExpiredToken(refreshTokenRequest.Token);
            var userName = userPrincipal.FindFirstValue(ClaimTypes.Name);
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new Exception("User Not Found.");
            }
            if (!await _userManager.IsInRoleAsync(user, RoleConstants.Internal))
            {
                throw new Exception("General Users not permitted");
            }
            if (user.IsDeleted)
            {
                throw new Exception("User Not Found.");
            }
            if (!user.IsActive)
            {
                throw new Exception("User Deactivated. Please contact the support.");
            }
            if (user.RefreshToken != refreshTokenRequest.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new Exception("Invalid Client Token.");
            }
            var token = await GenerateJwtAsync(user);
            user.RefreshToken = GenerateRefreshToken();
            await _userManager.UpdateAsync(user);

            var response = new TokenResponse { Token = token, RefreshToken = user.RefreshToken, RefreshTokenExpiryTime = user.RefreshTokenExpiryTime };
            return await Result<TokenResponse>.SuccessAsync(response);
        }
        catch (Exception e)
        {
            return await Result<TokenResponse>.FailAsync(e.Message);
        }
    }

    private async Task<string> GenerateJwtAsync(AppUser user)
    {
        return GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
    }

    private async Task<IEnumerable<Claim>> GetClaimsAsync(AppUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();
        foreach (var role in roles)
        {
            var currRoleClaims = await _roleManager.FindByNameAsync(role);
            roleClaims.AddRange(await _roleManager.GetClaimsAsync(currRoleClaims));
        }
        var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email ?? string.Empty),
                new(ClaimTypes.Name, user.UserName),
                new("FullName", user.FullName ?? string.Empty),
                new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
            }
            .Union(userClaims)
            .Union(roleClaims);
        return claims;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private static string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddDays(2), signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        var encryptedToken = tokenHandler.WriteToken(token);
        return encryptedToken;
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfiguration.Secret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var secret = Encoding.UTF8.GetBytes(_appConfiguration.Secret);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }
}