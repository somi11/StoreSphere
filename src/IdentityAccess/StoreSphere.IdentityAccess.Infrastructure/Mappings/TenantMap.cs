using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreSphere.IdentityAccess.Domain.Aggregates;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers; // Add this for clarity

namespace StoreSphere.IdentityAccess.Infrastructure.Mappings
{
        public class TenantMap : IEntityTypeConfiguration<Tenant>
        {
            public void Configure(EntityTypeBuilder<Tenant> builder)
            {
                builder.HasKey(t => t.Id);

                builder.Property(t => t.Name).IsRequired();
                builder.Property(t => t.OwnerId)
                    .HasConversion(
                        v => v.Value.ToString(),
                        v => new UserId(Guid.Parse(v))
                    )
                    .IsRequired();

                builder.Property(t => t.Status).IsRequired();

                builder.Ignore(t => t.Stores);
                builder.Ignore(t => t.Users);
                builder.Ignore(t => t.DomainEvents);
            }
        }
}



