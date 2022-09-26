namespace Infinite.Shared.Entities;

public class UserProfile
{
    public string Id { get; set; }
    public AppUser User { get; set; }
    public string UserId { get; set; }
    
    public string ProfileMarkdown { get; set; }
}