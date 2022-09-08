using Infinite.Client.Extensions;
using Infinite.Core.Interfaces.Services;

namespace Infinite.Server.Services;

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