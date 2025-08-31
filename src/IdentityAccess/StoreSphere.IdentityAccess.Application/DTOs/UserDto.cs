using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.DTOs
{
    public record UserDto(Guid UserId, string Email, string UserType, Guid? TenantId, bool IsActive);
}

