using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StoreSphere.IdentityAccess.Infrastructure.Persistence
{
    public class ApplicationUser : IdentityUser
    {
        // Extra columns for Identity if needed
    }

    public class ApplicationRole : IdentityRole
    {
        // Extra columns if needed
    }

    public class AppIdentityDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Set schema for all Identity tables
            builder.HasDefaultSchema("identityaccess");

            // Example: override table names if you want
            builder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("AspNetUsers", "identityaccess");
            });

            builder.Entity<ApplicationRole>(b =>
            {
                b.ToTable("AspNetRoles", "identityaccess");
            });

            // Do the same for Claims, Logins, Tokens, etc.
            builder.Entity<IdentityUserClaim<string>>()
                .ToTable("AspNetUserClaims", "identityaccess");
            builder.Entity<IdentityUserRole<string>>()
                .ToTable("AspNetUserRoles", "identityaccess");
            builder.Entity<IdentityUserLogin<string>>()
                .ToTable("AspNetUserLogins", "identityaccess");
            builder.Entity<IdentityRoleClaim<string>>()
                .ToTable("AspNetRoleClaims", "identityaccess");
            builder.Entity<IdentityUserToken<string>>()
                .ToTable("AspNetUserTokens", "identityaccess");
        }
    }
}
