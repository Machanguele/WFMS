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
        public DbSet<Department> Departments { get; set; }
        public DbSet<UserCrudAction> UserCrudActions { get; set; }
        public DbSet<UserInformationUpdate> UserInformationUpdates { get; set; }
        public DbSet<EcosystemInformationUpdate> EcosystemInformationUpdates { get; set; }
        public DbSet<ComponentStatus> ComponentStatus { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityStatus> ActivityStatuses { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<SetUp> SetUps { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<ActivityDependency> ActivityDependencies { get; set; }
        
    }
}