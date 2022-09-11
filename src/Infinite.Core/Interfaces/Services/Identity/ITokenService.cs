using Infinite.Shared.Requests.Identity;
using Infinite.Shared.Responses.Identity;
using Infinite.Shared.Wrapper;

namespace Infinite.Core.Interfaces.Services.Identity;

public interface ITokenService
{
    Task<IResult<TokenResponse>> LoginAsync(TokenRequest tokenRequest);

    Task<IResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
}