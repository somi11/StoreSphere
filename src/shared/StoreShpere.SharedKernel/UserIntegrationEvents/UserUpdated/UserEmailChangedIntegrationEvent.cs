using StoreShpere.SharedKernel.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreShpere.SharedKernel.UserIntegrationEvents.UserUpdated
{
    public class UserEmailChangedIntegrationEvent : IIntegrationEvent
    {
        public Guid UserId { get; }
        public string NewEmail { get; }

        public UserEmailChangedIntegrationEvent(Guid userId, string newEmail)
        {
            UserId = userId;
            NewEmail = newEmail;
        }
    }
}
