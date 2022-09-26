using Infinite.Shared.Entities;
using Infinite.Shared.Wrapper;

namespace Infinite.Core.Features;

public interface IBlogService
{
    Task<IResult<List<Blog>>> GetMyLast4Blogs(string userId);

    Task<IResult<Blog>> GetFullBlog(string id);

    Task<IResult> CreateBlog(Blog blog);

    Task<IResult> UpdateBlog(Blog blog);

    Task<IResult> DeleteBlog(string id);
}