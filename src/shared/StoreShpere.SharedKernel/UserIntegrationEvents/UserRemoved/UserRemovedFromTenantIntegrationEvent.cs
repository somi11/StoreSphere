using StoreShpere.SharedKernel.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreShpere.SharedKernel.UserIntegrationEvents.UserRemoved
{
    public class UserRemovedFromTenantIntegrationEvent : IIntegrationEvent
    {
        public Guid UserId { get; }
        public Guid TenantId { get; }

        public UserRemovedFromTenantIntegrationEvent(Guid userId, Guid tenantId)
        {
            UserId = userId;
            TenantId = tenantId;
        }
    }

}
