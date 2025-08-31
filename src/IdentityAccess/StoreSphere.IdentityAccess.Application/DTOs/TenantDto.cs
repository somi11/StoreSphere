using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.DTOs
{
    public record TenantDto(Guid TenantId, string Name, Guid OwnerUserId, string Status);

}
