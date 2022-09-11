using Infinite.Shared.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Infinite.Shared.Entities;

public class AppUser : IdentityUser, IAuditableEntity<string>
{
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string ProfilePictureDataUrl { get; set; }
    public bool IsActive { get; set; }

    public string CreatedBy { get; set; }
    public string CreatedByUserId { get; set; }
    public DateTime CreatedOn { get; set; }
    public string LastModifiedBy { get; set; }
    public string LastModifiedByUserId { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public bool IsDeleted { get; set; }
    
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}