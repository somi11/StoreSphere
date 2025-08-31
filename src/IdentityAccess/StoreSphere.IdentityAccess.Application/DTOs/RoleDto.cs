using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.DTOs
{
      public record RoleDto(Guid RoleId, string Name, IEnumerable<Guid> PermissionIds);

}
