using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreSphere.IdentityAccess.Domain.Aggregates;
using StoreSphere.IdentityAccess.Domain.Entities;
using StoreSphere.IdentityAccess.Infrastructure.Mappings;

namespace StoreSphere.IdentityAccess.Infrastructure.Persistence
{
    public class IdentityAccessDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public new DbSet<User> Users { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public new DbSet<Role> Roles { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<StoreUserAssignment> StoreUserAssignments { get; set; }
        public DbSet<UserRoleAssignment> UserRoleAssignments { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        public IdentityAccessDbContext(DbContextOptions<IdentityAccessDbContext> options)
           : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply mappings
            builder.ApplyConfiguration(new UserMap());
            builder.ApplyConfiguration(new TenantMap());
            builder.ApplyConfiguration(new StoreMap());
            builder.ApplyConfiguration(new RoleMap());
            builder.ApplyConfiguration(new PermissionMap());
            builder.ApplyConfiguration(new StoreUserAssignmentMap());
            builder.ApplyConfiguration(new UserRoleAssignmentMap());
            builder.ApplyConfiguration(new RolePermissionMap());
          

            // Example: Configure Email as owned type
            builder.Entity<User>().OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Value).HasColumnName("Email").IsRequired();
            });

            // Add other value object configurations as needed
        }
    }
}
