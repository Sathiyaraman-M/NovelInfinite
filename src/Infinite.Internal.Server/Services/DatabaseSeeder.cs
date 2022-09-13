using System.Security.Claims;
using Infinite.Core.Interfaces.Services;
using Infinite.Core.Persistence;
using Infinite.Shared.Constants;
using Infinite.Shared.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infinite.Internal.Server.Services;

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
        InitializeInternalUser();
        _appDbContext.SaveChanges();
    }

    private void InitializeInternalUser()
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
                    var roleResult = await _userManager.AddToRoleAsync(appUser, RoleConstants.Internal);
                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation("Added {FullName} to {Admin} role!", appUser.FullName, RoleConstants.Internal);
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
        }).GetAwaiter().GetResult();
    }

    private void InitializeRoles()
    {
        Task.Run(async () =>
        {
            if (!await _roleManager.Roles.AnyAsync())
            {
                var internalRole = new IdentityRole(RoleConstants.Internal);
                var internalRoleResult = await _roleManager.CreateAsync(internalRole);
                if (internalRoleResult.Succeeded)
                {
                    _logger.LogInformation("Seeded {Internal} role successfully!", RoleConstants.Internal);
                }
                else
                {
                    foreach (var error in internalRoleResult.Errors)
                    {
                        _logger.LogError("ErrorCode: {Code}\nDescription: {Description}", error.Code, error.Description);
                    }
                }

                var generalRoleResult = await _roleManager.CreateAsync(new IdentityRole(RoleConstants.General));
                if (generalRoleResult.Succeeded)
                {
                    _logger.LogInformation("Seeded {General} role successfully!", RoleConstants.General);
                }
                else
                {
                    foreach (var error in internalRoleResult.Errors)
                    {
                        _logger.LogError("ErrorCode: {Code}\nDescription: {Description}", error.Code, error.Description);
                    }
                }
            }

            var existingInternalRole = await _roleManager.FindByNameAsync(RoleConstants.Internal);
            var packedPermission = Enum.GetValues<AppPermissions>().Aggregate("", (current, permission) => current + (char)Convert.ChangeType(permission, typeof(char)));
            var claims = await _roleManager.GetClaimsAsync(existingInternalRole);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(existingInternalRole, claim);
            }
            await _roleManager.AddClaimAsync(existingInternalRole, new Claim(ApplicationClaimTypes.Permission, packedPermission));
            
        }).GetAwaiter().GetResult();
    }
}