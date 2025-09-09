using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StoreSphere.IdentityAccess.Infrastructure.Persistence
{
    public class AppIdentityDbContext
        : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Use custom schema
            builder.HasDefaultSchema("identityaccess");

            // Rename all Identity tables (optional)
            builder.Entity<IdentityUser>(b =>
            {
                b.ToTable("IdentityUsers", "identityaccess");
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable("IdentityRoles", "identityaccess");
            });

            builder.Entity<IdentityUserClaim<string>>()
                .ToTable("IdentityUserClaims", "identityaccess");

            builder.Entity<IdentityUserRole<string>>()
                .ToTable("IdentityUserRoles", "identityaccess");

            builder.Entity<IdentityUserLogin<string>>()
                .ToTable("IdentityUserLogins", "identityaccess");

            builder.Entity<IdentityRoleClaim<string>>()
                .ToTable("IdentityRoleClaims", "identityaccess");

            builder.Entity<IdentityUserToken<string>>()
                .ToTable("IdentityUserTokens", "identityaccess");
        }
    }
}
