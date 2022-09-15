namespace Infinite.Shared.Responses.Identity;

public class UserResponse
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; } = true;
    public bool EmailConfirmed { get; set; }
    public string PhoneNumber { get; set; }
    public string ProfilePictureDataUrl { get; set; }
}