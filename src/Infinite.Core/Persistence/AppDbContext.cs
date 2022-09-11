using Infinite.Shared.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infinite.Core.Persistence;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<AppUser>().ToTable("AppUsers", "dbo");
        builder.Entity<IdentityRole>().ToTable("AppRoles", "dbo");
        builder.Entity<IdentityUserRole<string>>().ToTable("AppUserRoles", "dbo");
        builder.Entity<IdentityUserClaim<string>>().ToTable("AppUserClaims", "dbo");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("AppRoleClaims", "dbo");
        builder.Ignore<IdentityUserLogin<string>>();
        builder.Ignore<IdentityUserToken<string>>();
    }
}