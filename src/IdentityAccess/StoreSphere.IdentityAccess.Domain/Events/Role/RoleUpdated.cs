
using StoreShpere.SharedKernel.Events;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Domain.Events.Role
{
    public record RoleUpdated(RoleId RoleId, string NewName) : IDomainEvent;

}
