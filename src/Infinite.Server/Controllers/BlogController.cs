using System.Security.Claims;
using IdentityModel;
using Infinite.Core.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Infinite.Server.Controllers;

[Authorize]
[Route("api/blog")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly IBlogService _blogService;

    public BlogController(IBlogService blogService)
    {
        _blogService = blogService;
    }
    
    [HttpGet("my-last-four")]
    public async Task<IActionResult> GetMyLast4Blogs()
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        return Ok(await _blogService.GetMyLast4Blogs(userId));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFullBlog(string id)
    {
        return Ok(await _blogService.GetFullBlog(id));
    }
}