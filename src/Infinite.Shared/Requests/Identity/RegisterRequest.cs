﻿using System.ComponentModel.DataAnnotations;

namespace Infinite.Shared.Requests.Identity;

public class RegisterRequest
{
    [Required]
    public string FullName { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }

    public string PhoneNumber { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public string City { get; set; }
    
    public string Country { get; set; }

    public bool ActivateUser { get; set; } = false;
    public bool AutoConfirmEmail { get; set; } = false;
}