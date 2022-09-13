using Infinite.Core.Interfaces.Services;
using Infinite.Internal.Client.Extensions;

namespace Infinite.Server.Internal.Services;

public class CurrentUserService : ICurrentUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        UserId = httpContextAccessor.HttpContext?.User.GetUserId();
        UserName = httpContextAccessor.HttpContext?.User.GetUserName();
    }

    public string UserId { get; set; }
    public string UserName { get; set; }
}