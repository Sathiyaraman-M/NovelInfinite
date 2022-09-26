using Duende.IdentityServer.EntityFramework.Options;
using Infinite.Shared.Entities;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infinite.Core.Persistence;

public class AppDbContext : ApiAuthorizationDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options,
        IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
    {
        
    }
}