using StoreShpere.SharedKernel.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreShpere.SharedKernel.UserIntegrationEvents.UserRemoved
{
    public class UserRemovedIntegrationEvent : IIntegrationEvent
    {
        public Guid UserId { get; }

        public UserRemovedIntegrationEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}
