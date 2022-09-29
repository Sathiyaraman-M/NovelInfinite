using Infinite.Core.Extensions;
using Infinite.Core.Interfaces.Repositories;
using Infinite.Shared.Entities;
using Infinite.Shared.Enums;
using Infinite.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infinite.Core.Features;

public class BlogService : IBlogService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;

    public BlogService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<IResult<List<Blog>>> GetMyLast4Blogs(string userId)
    {
        return await GetLastNBlogs(userId, false, 4);
    }

    public async Task<IResult<List<Blog>>> GetLastNBlogs(string userId, bool isOnlyPublic, int n)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User not found!");
            var blogs = await _unitOfWork.GetRepository<Blog>().Entities
                                                    .Where(x => x.UserId == userId)
                                                    .WhereIf(isOnlyPublic, x => x.Visibility == Visibility.Public)
                                                    .OrderByDescending(x => x.CreatedDate)
                                                    .Take(4)
                                                    .ToListAsync();

            return await Result<List<Blog>>.SuccessAsync(blogs);
        }
        catch (Exception e)
        {
            return await Result<List<Blog>>.FailAsync(e.Message);
        }
        
    }

    public async Task<IResult<Blog>> GetFullBlog(string id, string userId)
    {
        try
        {
            var blog = await _unitOfWork.GetRepository<Blog>().GetByIdAsync(id);
            if (blog.UserId != userId && blog.Visibility == Visibility.Private)
                throw new Exception("Cannot access the requested blog");
            return await Result<Blog>.SuccessAsync(blog);
        }
        catch (Exception e)
        {
            return await Result<Blog>.FailAsync(e.Message);
        }
    }

    public async Task<IResult> CreateBlog(Blog blog, string userId)
    {
        try
        {
            blog.Id = Guid.NewGuid().ToString();
            blog.UserId = userId;
            blog.AuthorName = (await _userManager.FindByIdAsync(userId)).UserName;
            blog.CreatedDate = DateTime.Now;
            if (string.IsNullOrEmpty(blog.Title))
                throw new Exception("Please enter blog title");
            if (string.IsNullOrEmpty(blog.Markdown))
                throw new Exception("Please add some content in the blog. Empty blog cannot be posted");
            await _unitOfWork.GetRepository<Blog>().AddAsync(blog);
            await _unitOfWork.Commit();
            return await Result.SuccessAsync();
        }
        catch (Exception e)
        {
            return await Result.FailAsync(e.Message);
        }
    }

    public async Task<IResult> UpdateBlog(Blog blog)
    {
        try
        {
            await _unitOfWork.Commit();
            return await Result.SuccessAsync();
        }
        catch (Exception e)
        {
            return await Result.FailAsync(e.Message);
        }
    }

    public async Task<IResult> DeleteBlog(string id)
    {
        try
        {
            await _unitOfWork.Commit();
            return await Result.SuccessAsync();
        }
        catch (Exception e)
        {
            return await Result.FailAsync(e.Message);
        }
    }
}