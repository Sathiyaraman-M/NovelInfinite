using System.Security.Claims;
using IdentityModel;
using Infinite.Core.Features;
using Infinite.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Infinite.Server.Controllers;

[Authorize]
[Route("api/blog-draft")]
[ApiController]
public class BlogDraftController : ControllerBase
{
    private readonly IBlogDraftService _blogDraftService;

    public BlogDraftController(IBlogDraftService blogDraftService)
    {
        _blogDraftService = blogDraftService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(int pageNumber, int pageSize, string searchString)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        return Ok(await _blogDraftService.GetBlogDrafts(pageNumber, pageSize, searchString, userId));
    }
    
    
    [HttpGet("recent")]
    public async Task<IActionResult> GetMyLastNBlogDrafts(int n = 4)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        return Ok(await _blogDraftService.GetMyLastNBlogDrafts(userId));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        return Ok(await _blogDraftService.GetBlogDraft(id));
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveToDraftAsync(BlogDraft draft)
    {
        var userId = HttpContext.User.FindFirstValue(JwtClaimTypes.Subject);
        return Ok(await _blogDraftService.SaveToDraft(draft, userId));
    }
}