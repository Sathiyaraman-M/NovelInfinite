using Infinite.Shared.Entities;
using Infinite.Shared.Responses;
using Infinite.Shared.Wrapper;

namespace Infinite.Core.Features;

public interface IBlogService
{
    Task<IResult<List<BlogResponse>>> GetMyLast4Blogs(string userId);
    Task<IResult<List<BlogResponse>>> GetLastNBlogs(string userId, bool isOnlyPublic, int n);
    Task<PaginatedResult<BlogResponse>> GetAllBlogs(int pageNumber, int pageSize, string searchString, string userId);
    Task<IResult<Blog>> GetFullBlog(string id, string userId);
    Task<IResult> CreateBlog(Blog blog, string userId);
    Task<IResult> UpdateBlog(Blog blog, string userId);
    Task<IResult> DeleteBlog(string id);
}