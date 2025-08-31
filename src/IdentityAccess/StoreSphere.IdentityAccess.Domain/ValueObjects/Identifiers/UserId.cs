using StoreSphere.IdentityAccess.Domain.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers
{
    public record UserId(Guid Value) : StronglyTypedId<Guid>(Value);
}
