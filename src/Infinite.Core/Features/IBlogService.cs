using Infinite.Shared.Entities;
using Infinite.Shared.Wrapper;

namespace Infinite.Core.Features;

public interface IBlogService
{
    Task<IResult<List<Blog>>> GetMyLast4Blogs(string userId);
    Task<IResult<List<Blog>>> GetLastNBlogs(string userId, bool isOnlyPublic, int n);
    Task<IResult<Blog>> GetFullBlog(string id, string userId);
    Task<IResult> CreateBlog(Blog blog, string userId);
    Task<IResult> UpdateBlog(Blog blog);
    Task<IResult> DeleteBlog(string id);
}