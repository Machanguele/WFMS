using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<AppUser> AppUsers { get; set; } 
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<EcoSystem> EcoSystems { get; set; }
        public DbSet<UserSystem> UserSystems { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationPermission> ApplicationPermissions { get; set; }
        public DbSet<ApplicationRolePermission> ApplicationRolePermissions { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<UserCrudAction> UserCrudActions { get; set; }
        public DbSet<UserInformationUpdate> UserInformationUpdates { get; set; }
        public DbSet<EcosystemInformationUpdate> EcosystemInformationUpdates { get; set; }
    }
}