using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.DTOs
{
    public record StoreDto(Guid StoreId, Guid TenantId, string Name, string? DomainName, string Status);

}
