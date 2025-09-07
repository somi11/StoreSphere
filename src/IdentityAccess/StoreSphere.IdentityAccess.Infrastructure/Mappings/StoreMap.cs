using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreSphere.IdentityAccess.Domain.Aggregates;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;

namespace StoreSphere.IdentityAccess.Infrastructure.Mappings
{
    public class StoreMap : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.TenantId)
                .HasConversion(
                    v => v.Value.ToString(),
                    v => new TenantId(Guid.Parse(v))
                )
                .IsRequired();

            builder.Property(s => s.Name).IsRequired();
            builder.Property(s => s.DomainName);
            builder.Property(s => s.Status).IsRequired();

            builder.Ignore(s => s.Assignments); // StoreUserAssignment mapped separately
            builder.Ignore(s => s.DomainEvents);
        }
    }
}
