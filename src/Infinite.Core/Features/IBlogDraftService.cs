using Infinite.Shared.Entities;
using Infinite.Shared.Responses;
using Infinite.Shared.Wrapper;

namespace Infinite.Core.Features;

public interface IBlogDraftService
{
    Task<IResult<List<BlogDraftResponse>>> GetMyLastNBlogDrafts(string userId, int n = 4);
    
    Task<PaginatedResult<BlogDraft>> GetBlogDrafts(int pageNumber, int pageSize, string searchString, string userId);

    Task<IResult<BlogDraft>> GetBlogDraft(string id);
    
    Task<IResult> SaveToDraft(BlogDraft draft, string userId);
}