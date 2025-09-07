using StoreShpere.SharedKernel.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreShpere.SharedKernel.UserIntegrationEvents.UserUpdated
{
    public class UserActivatedIntegrationEvent : IIntegrationEvent
    {
        public Guid UserId { get; }

        public UserActivatedIntegrationEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}
