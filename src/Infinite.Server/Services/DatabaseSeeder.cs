using Infinite.Core.Interfaces.Services;
using Infinite.Core.Persistence;
using Infinite.Shared.Constants;
using Infinite.Shared.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infinite.Server.Services;

public class DatabaseSeeder : IDatabaseSeeder
{
    private readonly AppDbContext _appDbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(AppDbContext appDbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<DatabaseSeeder> logger)
    {
        _appDbContext = appDbContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public void Initialize()
    {
        InitializeRoles();
        InitializeAdminUser();
        _appDbContext.SaveChanges();
    }

    private void InitializeAdminUser()
    {
        Task.Run(async () =>
        {
            if (!await _userManager.Users.AnyAsync())
            {
                var appUser = new AppUser
                {
                    UserName = UserConstants.Admin,
                    FullName = "Administrator",
                    Email = "admin@novel-infinite.com",
                    EmailConfirmed = true,
                    IsActive = true
                };
                var result = await _userManager.CreateAsync(appUser, UserConstants.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Seeded {FullName} user successfully!", appUser.FullName);
                    var roleResult = await _userManager.AddToRoleAsync(appUser, RoleConstants.Admin);
                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation("Added {FullName} to {Admin} role!", appUser.FullName, RoleConstants.Admin);
                    }
                    else
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            _logger.LogError("ErrorCode: {Code}\nDescription: {Description}", error.Code, error.Description);
                        }
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError("ErrorCode: {Code}\nDescription: {Description}", error.Code, error.Description);
                    }
                }
            }
        });
    }

    private void InitializeRoles()
    {
        Task.Run(async () =>
        {
            if (!await _roleManager.Roles.AnyAsync())
            {
                var roles = new[]
                {
                    new IdentityRole(RoleConstants.Admin),
                    new IdentityRole(RoleConstants.Faculty),
                    new IdentityRole(RoleConstants.Student)
                };
                foreach (var role in roles)
                {
                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Seeded {Name} role successfully!", role.Name);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            _logger.LogError("ErrorCode: {Code}\nDescription: {Description}", error.Code, error.Description);
                        }
                    }
                }
            }
        });
    }
}