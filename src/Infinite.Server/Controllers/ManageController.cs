using System.Security.Claims;
using IdentityModel;
using Infinite.Core.Features;
using Infinite.Shared.Requests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Infinite.Server.Controllers;

[Authorize]
[Route("api/manage")]
[ApiController]
public class ManageController : ControllerBase
{
    private readonly IManageAccountService _manageAccountService;

    public ManageController(IManageAccountService manageAccountService)
    {
        _manageAccountService = manageAccountService;
    }
    
    [HttpGet("portfolio")]
    public async Task<IActionResult> GetCurrentUserPortFolio()
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        return Ok(await _manageAccountService.GetPortFolioMd(userId));
    }

    [HttpPost("portfolio")]
    public async Task<IActionResult> SaveCurrentUserPortFolio(MarkdownModel model)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        return Ok(await _manageAccountService.SavePortFolio(userId, model.Markdown));
    }
    
    [HttpGet("profileInfo")]
    public async Task<IActionResult> GetCurrentUserProfileInfo()
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        return Ok(await _manageAccountService.GetUserProfileInfo(userId));
    }

    [HttpPost("profileInfo")]
    public async Task<IActionResult> SaveCurrentUserProfileInfo(UpdateUserProfileInfoRequest request)
    {
        return Ok(await _manageAccountService.UpdateUserProfileInfo(request));
    }

    [HttpDelete("account")]
    public async Task<IActionResult> DeleteInfiniteAccount()
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        await HttpContext.SignOutAsync();
        return Ok(await _manageAccountService.DeleteInfiniteAccount(userId));
    }

    public class MarkdownModel
    {
        public string Markdown { get; set; }
    }
}