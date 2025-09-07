using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreSphere.IdentityAccess.Domain.Aggregates;
using StoreSphere.IdentityAccess.Domain.ValueObjects.RoleScope;

namespace StoreSphere.IdentityAccess.Infrastructure.Mappings
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name).IsRequired();

            builder.Property(r => r.Scope)
                .HasConversion<int>() // Store enum as int
                .IsRequired();

            //builder.Ignore(r => r.Permissions); // Permissions handled elsewhere
            builder.Ignore(r => r.DomainEvents);
        }
    }
}
