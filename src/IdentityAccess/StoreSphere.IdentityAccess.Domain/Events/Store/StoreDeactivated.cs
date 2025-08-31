
using StoreShpere.SharedKernel.Events;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Domain.Events.Store
{
    public record StoreDeactivated(StoreId StoreId) : IDomainEvent;
}
