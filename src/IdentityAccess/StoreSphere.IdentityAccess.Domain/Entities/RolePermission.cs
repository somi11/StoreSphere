using StoreSphere.IdentityAccess.Domain.Aggregates;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Domain.Entities
{
    public class RolePermission
    {
        public RoleId RoleId { get; set; }
        public PermissionId PermissionId { get; set; }

        // Optional navigation properties for EF Core convenience
       public Role Role { get; set; }
       public Permission Permission { get; set; }
    }
}
