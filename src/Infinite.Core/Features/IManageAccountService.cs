using Infinite.Core.Persistence.Migrations;
using Infinite.Shared.Requests;
using Infinite.Shared.Responses;
using Infinite.Shared.Wrapper;

namespace Infinite.Core.Features;

public interface IManageAccountService
{
    Task<IResult<string>> GetPortFolioMd(string userId);

    Task<IResult> SavePortFolio(string userId, string markdown);

    Task<IResult<UserProfileInfoResponse>> GetUserProfileInfo(string userId);
    
    Task<IResult> UpdateUserProfileInfo(UpdateUserProfileInfoRequest request);

    Task<IResult> DeleteInfiniteAccount(string userId);
}