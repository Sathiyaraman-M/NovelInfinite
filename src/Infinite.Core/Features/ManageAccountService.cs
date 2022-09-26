using Infinite.Core.Interfaces.Repositories;
using Infinite.Shared.Entities;
using Infinite.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace Infinite.Core.Features;

public class ManageAccountService : IManageAccountService
{
    private readonly IUnitOfWork _unitOfWork;

    public ManageAccountService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult<string>> GetPortFolioMd(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User not found!");
            var portFolioMd = await _unitOfWork.GetRepository<UserProfile>().Entities
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (portFolioMd != null) 
                return await Result<string>.SuccessAsync(portFolioMd.ProfileMarkdown);
            portFolioMd = new UserProfile()
            {
                Id = Guid.NewGuid().ToString(),
                ProfileMarkdown = string.Empty,
                UserId = userId
            };
            await _unitOfWork.GetRepository<UserProfile>().AddAsync(portFolioMd);
            await _unitOfWork.Commit();
            return await Result<string>.SuccessAsync(portFolioMd.ProfileMarkdown);
        }
        catch (Exception e)
        {
            return await Result<string>.FailAsync(e.Message);
        }
    }
}