using Infinite.Client.Extensions;
using Infinite.Core.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Infinite.Server.Controllers;

[Authorize]
[Route("api/manage")]
[ApiController]
public class ManageController : ControllerBase
{
    private readonly IManageAccountService _manageAccountService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ManageController(IManageAccountService manageAccountService, IHttpContextAccessor httpContextAccessor)
    {
        _manageAccountService = manageAccountService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    [HttpGet("portfolio")]
    public async Task<IActionResult> GetCurrentUserPortFolio()
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
        return Ok(await _manageAccountService.GetPortFolioMd(userId));
    }
}