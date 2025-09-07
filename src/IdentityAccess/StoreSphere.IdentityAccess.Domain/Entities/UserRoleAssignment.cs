using StoreSphere.IdentityAccess.Domain.common;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using StoreSphere.IdentityAccess.Domain.ValueObjects.RoleScope;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Domain.Entities
{
    public class UserRoleAssignment : Entity<UserRoleAssignmentId>
    {
        public UserId UserId { get; private set; }
        public RoleId RoleId { get; private set; }
        public DateTime AssignedAt { get; private set; }

        private UserRoleAssignment() { } // EF Core

        public UserRoleAssignment(UserRoleAssignmentId id, UserId userId, RoleId roleId, RoleScope scope)
        {
            if (scope != RoleScope.Global)
                throw new InvalidOperationException("Only global roles can be assigned directly to a user.");

            Id = id;
            UserId = userId;
            RoleId = roleId;
            AssignedAt = DateTime.UtcNow;
        }
    }
}
