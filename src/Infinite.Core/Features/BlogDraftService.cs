using Infinite.Core.Extensions;
using Infinite.Core.Interfaces.Repositories;
using Infinite.Shared.Entities;
using Infinite.Shared.Responses;
using Infinite.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace Infinite.Core.Features;

public class BlogDraftService : IBlogDraftService
{
    private readonly IUnitOfWork _unitOfWork;

    public BlogDraftService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult<List<BlogDraftResponse>>> GetMyLastNBlogDrafts(string userId, int n = 4)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User not found!");
            var blogDrafts = await _unitOfWork.GetRepository<BlogDraft>().Entities
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.SaveDateTime)
                .Take(n)
                .Select(x => new BlogDraftResponse
                {
                    Id = x.Id,
                    AuthorName = x.AuthorName,
                    SaveDateTime = x.SaveDateTime,
                    Title = x.Title,
                    BlogId = x.BlogId
                })
                .ToListAsync();
            return await Result<List<BlogDraftResponse>>.SuccessAsync(blogDrafts);
        }
        catch (Exception e)
        {
            return await Result<List<BlogDraftResponse>>.FailAsync(e.Message);
        }
    }

    public async Task<PaginatedResult<BlogDraft>> GetBlogDrafts(int pageNumber, int pageSize, string searchString, string userId)
    {
        try
        {
            searchString = searchString.ToLower();
            return await _unitOfWork.GetRepository<BlogDraft>()
                .Entities
                .Where(x => x.Title.ToLower().Contains(searchString) && x.UserId == userId)
                .ToPaginatedListAsync(pageNumber, pageSize);
        }
        catch (Exception e)
        {
            return PaginatedResult<BlogDraft>.Failure(new List<string>() { e.Message });
        }
    }

    public async Task<IResult<BlogDraft>> GetBlogDraft(string id)
    {
        try
        {
            var draft = await _unitOfWork.GetRepository<BlogDraft>().GetByIdAsync(id);
            return await Result<BlogDraft>.SuccessAsync(draft);
        }
        catch (Exception e)
        {
            return await Result<BlogDraft>.FailAsync(e.Message);
        }
    }

    public async Task<IResult> SaveToDraft(BlogDraft draft, string userId)
    {
        try
        {
            var existingDraft = await _unitOfWork.GetRepository<BlogDraft>().Entities
                .FirstOrDefaultAsync(x => x.BlogId == draft.BlogId);
            draft.SaveDateTime = DateTime.Now;
            draft.UserId = userId;
            draft.AuthorName = (await _unitOfWork.GetRepository<UserProfileInfo>().Entities.FirstAsync(x => x.UserId == userId)).FullName;
            if (existingDraft == null)
            {
                draft.Id = Guid.NewGuid().ToString();
                await _unitOfWork.GetRepository<BlogDraft>().AddAsync(draft);
            }
            else
            {
                await _unitOfWork.GetRepository<BlogDraft>().UpdateAsync(draft, existingDraft.Id);
            }
            await _unitOfWork.Commit();
            return await Result.SuccessAsync();
        }
        catch (Exception e)
        {
            return await Result.FailAsync(e.Message);
        }
    }
}