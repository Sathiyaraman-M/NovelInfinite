using Infinite.Shared.Responses.Identity;

namespace Infinite.Shared.Requests.Identity;

public class UpdateUserRolesRequest
{
    public string UserId { get; set; }
    public IList<UserRoleModel> UserRoles { get; set; }
}