using StoreShpere.SharedKernel.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreShpere.SharedKernel.UserIntegrationEvents.UserRemoved
{

    public class UserRemovedFromStoreIntegrationEvent : IIntegrationEvent
    {
        public Guid UserId { get; }
        public Guid StoreId { get; }
        public Guid RoleId { get; }

        public UserRemovedFromStoreIntegrationEvent(Guid userId, Guid storeId, Guid roleId)
        {
            UserId = userId;
            StoreId = storeId;
            RoleId = roleId;
        }
    }
}
