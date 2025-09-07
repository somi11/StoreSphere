using StoreShpere.SharedKernel.Events;

namespace StoreShpere.SharedKernel.UserIntegrationEvents.UserRegistered
{
    public class UserRegisteredIntegrationEvent : IIntegrationEvent
    {
        public Guid UserId { get; }
        public string Email { get; }
        public string UserType { get; }
        public Guid? TenantId { get; }
        public string? IdentityId { get; }

        public UserRegisteredIntegrationEvent(Guid userId, string email, string userType, Guid? tenantId, string? identityId)
        {
            UserId = userId;
            Email = email;
            UserType = userType;
            TenantId = tenantId;
            IdentityId = identityId;
        }
    }
}
