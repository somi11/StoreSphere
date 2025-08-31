using StoreShpere.SharedKernel.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.Events.User.UserRegisteredEvents.IntegrationEvent
{
    public class UserRegisteredIntegrationEvent : IIntegrationEvent
    {
        public Guid UserId { get; }
        public string Email { get; }
        public string UserType { get; }
        public Guid? TenantId { get; }

        public UserRegisteredIntegrationEvent(Guid userId, string email, string userType, Guid? tenantId)
        {
            UserId = userId;
            Email = email;
            UserType = userType;
            TenantId = tenantId;
        }
    }
}
