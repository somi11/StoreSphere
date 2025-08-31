
using StoreShpere.SharedKernel.Events;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Accounts;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Domain.Events.Tenant
{
    public record TenantRegistered(TenantId TenantId , string Name, UserId OwnerId) : IDomainEvent;
}
