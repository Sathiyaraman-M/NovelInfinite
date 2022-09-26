namespace Infinite.Shared.Entities;

public class UserProfileInfo
{
    public string Id { get; set; }
    public AppUser User { get; set; }
    public string UserId { get; set; }
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string ProfilePictureDataUrl { get; set; }
    
    public string AboutMe { get; set; }
    public string Status { get; set; }
}