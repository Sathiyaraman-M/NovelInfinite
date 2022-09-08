namespace Infinite.Core.Interfaces.Services;

public interface ICurrentUserService
{
    string UserId { get; set; }
    string UserName { get; set; }
}