using System.Security.Claims;
using IdentityModel;
using Infinite.Core.Features;
using Infinite.Shared.Entities;
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
    
    [AllowAnonymous]
    [HttpGet("recent")]
    public async Task<IActionResult> GetRecentBlogsFromAuthor(string id, int n = 4)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        return Ok(await _blogService.GetLastNBlogs(userId, id != userId, n));
    }
    
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFullBlog(string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        return Ok(await _blogService.GetFullBlog(id, userId));
    }

    [HttpPost("personal")]
    public async Task<IActionResult> CreateNewBlogPersonal(Blog blog)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        return Ok(await _blogService.CreateBlog(blog, userId));
    }

    [HttpPut("personal/{id}")]
    public async Task<IActionResult> UpdateBlogPersonal(Blog blog, string id)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        return Ok(await _blogService.UpdateBlog(blog, userId));
    }

    [HttpDelete("personal/{id}")]
    public async Task<IActionResult> DeleteBlogPersonal(string id)
    {
        return Ok(await _blogService.DeleteBlog(id));
    }
}