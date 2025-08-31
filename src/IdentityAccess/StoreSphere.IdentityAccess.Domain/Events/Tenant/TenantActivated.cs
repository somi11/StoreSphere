using StoreShpere.SharedKernel.Events;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;


namespace StoreSphere.IdentityAccess.Domain.Events.Tenant
{
    public  record TenantActivated(TenantId TenantId) : IDomainEvent
    {
    }
}
