
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreSphere.IdentityAccess.Domain.Entities;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;

namespace StoreSphere.IdentityAccess.Infrastructure.Mappings
{
    public class StoreUserAssignmentMap : IEntityTypeConfiguration<StoreUserAssignment>
    {
        public void Configure(EntityTypeBuilder<StoreUserAssignment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasConversion(
                    v => v.Value.ToString(),
                    v => new StoreUserAssignmentId(Guid.Parse(v))
                )
                .IsRequired();

            builder.Property(a => a.StoreId)
                .HasConversion(
                    v => v.Value.ToString(),
                    v => new StoreId(Guid.Parse(v))
                )
                .IsRequired();

            builder.Property(a => a.UserId)
                .HasConversion(
                    v => v.Value.ToString(),
                    v => new UserId(Guid.Parse(v))
                )
                .IsRequired();

            builder.Property(a => a.RoleId)
                .HasConversion(
                    v => v.Value.ToString(),
                    v => new RoleId(Guid.Parse(v))
                )
                .IsRequired();

            builder.Property(a => a.AssignedAt)
                .IsRequired();

            // builder.Ignore(a => a.DomainEvents); // Uncomment if you add DomainEvents property
        }
    }
}
