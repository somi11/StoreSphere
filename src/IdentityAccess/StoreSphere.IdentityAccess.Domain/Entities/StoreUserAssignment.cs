using StoreSphere.IdentityAccess.Domain.common;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Domain.Entities
{
    public class StoreUserAssignment : Entity<StoreUserAssignmentId>
    {
        public StoreId StoreId { get; private set; }
        public UserId UserId { get; private set; }
        public RoleId RoleId { get; private set; }
        public DateTime AssignedAt { get; private set; }

        private StoreUserAssignment() { } // EF Core

        public StoreUserAssignment(StoreUserAssignmentId id, StoreId storeId, UserId userId, RoleId roleId)
        {
            Id = id;
            StoreId = storeId;
            UserId = userId;
            RoleId = roleId;
            AssignedAt = DateTime.UtcNow;
        }
    }
}
