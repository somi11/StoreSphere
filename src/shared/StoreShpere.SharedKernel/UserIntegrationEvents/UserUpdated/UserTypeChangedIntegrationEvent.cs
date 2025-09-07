using StoreShpere.SharedKernel.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreShpere.SharedKernel.UserIntegrationEvents.UserUpdated
{

    public class UserTypeChangedIntegrationEvent : IIntegrationEvent
    {
        public Guid UserId { get; }
        public string NewUserType { get; }

        public UserTypeChangedIntegrationEvent(Guid userId, string newUserType)
        {
            UserId = userId;
            NewUserType = newUserType;
        }
    }

}
