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
            var portFolioMd = await _unitOfWork.GetRepository<UserPortfolio>().Entities
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (portFolioMd != null) 
                return await Result<string>.SuccessAsync(portFolioMd.PortfolioMarkdown);
            portFolioMd = new UserPortfolio()
            {
                Id = Guid.NewGuid().ToString(),
                PortfolioMarkdown = string.Empty,
                UserId = userId
            };
            await _unitOfWork.GetRepository<UserPortfolio>().AddAsync(portFolioMd);
            await _unitOfWork.Commit();
            return await Result<string>.SuccessAsync(portFolioMd.PortfolioMarkdown);
        }
        catch (Exception e)
        {
            return await Result<string>.FailAsync(e.Message);
        }
    }
}