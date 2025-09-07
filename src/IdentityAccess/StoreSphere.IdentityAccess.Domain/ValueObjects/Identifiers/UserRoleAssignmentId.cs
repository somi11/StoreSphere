using StoreSphere.IdentityAccess.Domain.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers
{
    public record UserRoleAssignmentId(Guid Value) : StronglyTypedId<Guid>(Value)
    {
        public static UserRoleAssignmentId New()
        {
            return new UserRoleAssignmentId(Guid.NewGuid());
        }
    }
}
