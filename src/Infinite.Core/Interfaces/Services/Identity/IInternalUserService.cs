using Infinite.Shared.Entities;
using Infinite.Shared.Requests.Identity;
using Infinite.Shared.Responses.Identity;
using Infinite.Shared.Wrapper;

namespace Infinite.Core.Interfaces.Services.Identity;

public interface IInternalUserService
{
    Task<Result<List<UserResponse>>> GetAllAsync();
    Task<int> GetCountAsync();
    Task<IResult<UserResponse>> GetAsync(string userId);
    Task<IResult> RegisterAsync(RegisterRequest request, string origin);
    Task<string> SendVerificationEmail(AppUser user, string origin);
    Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request);
    Task<IResult<UserRolesResponse>> GetRolesAsync(string id);
    Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request);
    Task<IResult<string>> ConfirmEmailAsync(string userId, string code);
    Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
    Task<IResult> ResetPasswordAsync(ResetPasswordRequest request);
}