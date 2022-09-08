using System.ComponentModel.DataAnnotations;

namespace Infinite.Shared.Requests.Identity;

public class TokenRequest
{
    [Required(ErrorMessage = "Please enter your username")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Please enter your password")]
    public string Password { get; set; }
}