using StoreShpere.SharedKernel.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreShpere.SharedKernel.UserIntegrationEvents.UserUpdated
{
    public class UserDeactivatedIntegrationEvent : IIntegrationEvent
    {
        public Guid UserId { get; }

        public UserDeactivatedIntegrationEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}
