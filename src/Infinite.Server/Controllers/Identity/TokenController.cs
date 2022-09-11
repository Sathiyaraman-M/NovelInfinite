using Infinite.Core.Interfaces.Services.Identity;
using Infinite.Shared.Requests.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Infinite.Server.Controllers.Identity;

[ApiController]
[Route("api/token")]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService) => _tokenService = tokenService;

    [HttpPost("get")]
    public async Task<IActionResult> LoginAsync(TokenRequest tokenRequest)
    {
        return Ok(await _tokenService.LoginAsync(tokenRequest));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        return Ok(await _tokenService.RefreshTokenAsync(refreshTokenRequest));
    }
}