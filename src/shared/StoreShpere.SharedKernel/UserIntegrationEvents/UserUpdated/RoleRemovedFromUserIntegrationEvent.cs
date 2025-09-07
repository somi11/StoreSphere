using StoreShpere.SharedKernel.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreShpere.SharedKernel.UserIntegrationEvents.UserUpdated
{
    public class RoleRemovedFromUserIntegrationEvent : IIntegrationEvent
    {
        public Guid UserId { get; }
        public Guid RoleId { get; }

        public RoleRemovedFromUserIntegrationEvent(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}
