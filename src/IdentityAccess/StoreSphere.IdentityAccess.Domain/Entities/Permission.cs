using StoreSphere.IdentityAccess.Domain.common;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Domain.Entities
{
    public class Permission : Entity<PermissionId>
    {
        public string Name { get; private set; }
        private Permission() { }

        public Permission(PermissionId id, string name)
        {
            Id = id;
            Name = name;
           
        }
    }
}
