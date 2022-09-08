using System.ComponentModel.DataAnnotations;

namespace Infinite.Shared.Requests.Identity;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}