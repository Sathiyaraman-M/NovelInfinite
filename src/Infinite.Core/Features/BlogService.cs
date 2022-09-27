using Infinite.Core.Interfaces.Repositories;
using Infinite.Shared.Entities;
using Infinite.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace Infinite.Core.Features;

public class BlogService : IBlogService
{
    private readonly IUnitOfWork _unitOfWork;

    public BlogService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult<List<Blog>>> GetMyLast4Blogs(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User not found!");
            var blogs = await _unitOfWork.GetRepository<Blog>().Entities
                                                    .Where(x => x.UserId == userId)
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

    public async Task<IResult<Blog>> GetFullBlog(string id)
    {
        try
        {
            var blog = await _unitOfWork.GetRepository<Blog>().GetByIdAsync(id);
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