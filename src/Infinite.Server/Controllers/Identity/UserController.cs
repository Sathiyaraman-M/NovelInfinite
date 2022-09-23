using Infinite.Core.Interfaces.Services.Identity;
using Infinite.Shared.Requests.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Infinite.Server.Controllers.Identity;

[Authorize]
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IGeneralUserService _generalUserService;

    public UserController(IGeneralUserService generalUserService)
    {
        _generalUserService = generalUserService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request)
    {
        return Ok(await _generalUserService.RegisterAsync(request, Request.Headers["origin"]));
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        return Ok(await _generalUserService.ResetPasswordAsync(request));
    }
}