using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StoreSphere.IdentityAccess.Domain.Aggregates;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;

namespace StoreSphere.IdentityAccess.Infrastructure.Mappings
{

        public class UserMap : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.HasKey(u => u.Id);

                builder.OwnsOne(u => u.Email, email =>
                {
                    email.Property(e => e.Value).HasColumnName("Email").IsRequired();
                });


                builder.Property(u => u.IdentityId)
                    .HasColumnName("IdentityId")
                    .IsRequired(false); // IdentityId can be null

            builder.Property(u => u.IsActive).IsRequired();
                builder.Property(u => u.UserType).IsRequired();

                builder.Property(u => u.TenantId)
                .HasConversion(
                    v => v == null ? null : v.Value.ToString(),
                    v => v == null ? null : new TenantId(Guid.Parse(v))
                )
                .HasColumnName("TenantId");


            builder.Ignore(u => u.DomainEvents);

                // Ignore role assignments collection, handled via UserRoleAssignment entity
                builder.Ignore(u => u.RoleAssignments);
            }
        }
    


}
