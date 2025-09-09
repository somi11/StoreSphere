using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoreSphere.IdentityAccess.Domain.Aggregates;
using StoreSphere.IdentityAccess.Domain.common;
using StoreSphere.IdentityAccess.Domain.Entities;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Accounts;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using System.Linq.Expressions;
using System.Reflection;

namespace StoreSphere.IdentityAccess.Infrastructure.Persistence
{
    public class DomainDbContext : DbContext
    {
        public DomainDbContext(DbContextOptions<DomainDbContext> options)
            : base(options) { }

        // --- Aggregates ---
        public DbSet<User> Users { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Role> Roles { get; set; }

        // --- Entities ---
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRoleAssignment> UserRoleAssignments { get; set; }
        public DbSet<StoreUserAssignment> StoreUserAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ApplyStronglyTypedIdConversions(modelBuilder);
            // ---------- User ----------
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Id)
                      .HasConversion(id => id.Value, value => new UserId(value));

                entity.Property(u => u.Email)
                      .HasConversion(email => email.Value, value => Email.Create(value))
                      .IsRequired();

                entity.Property(u => u.UserType)
                      .HasConversion<string>();

                entity.Property(u => u.IdentityId)
                      .HasMaxLength(450);

                entity.Property(u => u.IsActive);

                // Navigation with backing field
                entity.HasMany(u => u.RoleAssignments)   // use property here
                      .WithOne()
                      .HasForeignKey("UserId")
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);

                // Tell EF to use the private field for persistence
                entity.Metadata
                      .FindNavigation(nameof(User.RoleAssignments))!
                      .SetPropertyAccessMode(PropertyAccessMode.Field);
            });
            // ---------- Tenant ----------
            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.ToTable("Tenants");

                entity.HasKey(t => t.Id);

                entity.Property(t => t.Id)
                      .HasConversion(id => id.Value, value => new TenantId(value));

                entity.Property(t => t.Name).IsRequired().HasMaxLength(200);

                entity.Property(t => t.Status)
                      .HasConversion<int>();

                // collections stored as separate tables
                entity.Ignore(t => t.Stores);
                entity.Ignore(t => t.Users);
            });

            // ---------- Store ----------
            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("Stores");

                entity.HasKey(s => s.Id);

                entity.Property(s => s.Id)
                      .HasConversion(id => id.Value, value => new StoreId(value));

                entity.Property(s => s.TenantId)
                      .HasConversion(id => id.Value, value => new TenantId(value));

                entity.Property(s => s.Name).IsRequired().HasMaxLength(200);

                entity.Property(s => s.DomainName).HasMaxLength(200);

                entity.Property(s => s.Status)
                      .HasConversion<int>();

                entity.HasMany(s => s.Assignments)
                      .WithOne()
                      .HasForeignKey(a => a.StoreId);
            });
            // ---------- Role ----------
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");

                entity.HasKey(r => r.Id);

                entity.Property(r => r.Id)
                      .HasConversion(id => id.Value, value => new RoleId(value));

                entity.Property(r => r.Name)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(r => r.Scope)
                      .HasConversion<int>();

                // Expose backing field for RolePermissions collection
                entity.Metadata
                      .FindNavigation(nameof(Role.RolePermissions))!
                      .SetPropertyAccessMode(PropertyAccessMode.Field);
            });


            // ---------- Permission ----------
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("Permissions");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                      .HasConversion(id => id.Value, value => new PermissionId(value));

                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            });

            // ---------- RolePermission ----------
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("RolePermissions");

                // Composite key (RoleId + PermissionId)
                entity.HasKey(rp => new { rp.RoleId, rp.PermissionId });

                // Relationship back to Role aggregate
                entity.HasOne(rp => rp.Role)
                      .WithMany(r => r.RolePermissions) // <-- corrected
                      .HasForeignKey(rp => rp.RoleId);

                // Relationship to Permission
                entity.HasOne(rp => rp.Permission)
                      .WithMany()
                      .HasForeignKey(rp => rp.PermissionId);
            });

            // ---------- UserRoleAssignment ----------
            modelBuilder.Entity<UserRoleAssignment>(entity =>
            {
                entity.ToTable("UserRoleAssignments");

                entity.HasKey(ura => ura.Id);

                entity.Property(ura => ura.Id)
                      .HasConversion(id => id.Value, value => new UserRoleAssignmentId(value));

                entity.Property(ura => ura.UserId)
                      .HasConversion(id => id.Value, value => new UserId(value));

                entity.Property(ura => ura.RoleId)
                      .HasConversion(id => id.Value, value => new RoleId(value));

                entity.Property(ura => ura.AssignedAt);
            });

            // ---------- StoreUserAssignment ----------
            modelBuilder.Entity<StoreUserAssignment>(entity =>
            {
                entity.ToTable("StoreUserAssignments");

                entity.HasKey(sua => sua.Id);

                entity.Property(sua => sua.Id)
                      .HasConversion(id => id.Value, value => new StoreUserAssignmentId(value));

                entity.Property(sua => sua.StoreId)
                      .HasConversion(id => id.Value, value => new StoreId(value));

                entity.Property(sua => sua.UserId)
                      .HasConversion(id => id.Value, value => new UserId(value));

                entity.Property(sua => sua.RoleId)
                      .HasConversion(id => id.Value, value => new RoleId(value));

                entity.Property(sua => sua.AssignedAt);
            });
        }


        private static void ApplyStronglyTypedIdConversions(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.ClrType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    var propertyType = property.PropertyType;

                    // Only process StronglyTypedId<T>
                    if (propertyType.BaseType is { IsGenericType: true } &&
                        propertyType.BaseType.GetGenericTypeDefinition() == typeof(StronglyTypedId<>))
                    {
                        var valueType = propertyType.BaseType.GetGenericArguments()[0];

                        // Build a ValueConverter dynamically: StronglyTypedId<T> <-> T
                        var converterType = typeof(ValueConverter<,>).MakeGenericType(propertyType, valueType);

                        var idParam = Expression.Parameter(propertyType, "id");
                        var valueParam = Expression.Parameter(valueType, "value");

                        var toProvider = Expression.Lambda(
                            Expression.Property(idParam, "Value"),
                            idParam
                        );

                        var fromProvider = Expression.Lambda(
                            Expression.New(propertyType.GetConstructor(new[] { valueType })!, valueParam),
                            valueParam
                        );

                        var converter = (ValueConverter)Activator.CreateInstance(
                            converterType,
                            toProvider,
                            fromProvider,
                            null
                        )!;

                        modelBuilder
                            .Entity(entityType.ClrType)
                            .Property(property.Name)
                            .HasConversion(converter);
                    }
                }
            }
        }

    }


}
