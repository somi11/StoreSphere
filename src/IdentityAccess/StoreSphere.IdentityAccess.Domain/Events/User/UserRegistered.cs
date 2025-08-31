using StoreShpere.SharedKernel.Events;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Accounts;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using StoreSphere.IdentityAccess.Domain.ValueObjects.UserType;

namespace StoreSphere.IdentityAccess.Domain.Events.User
{
    public record UserRegistered(UserId UserId, UserType UserType, Email Email, TenantId? TenantId) : IDomainEvent;

}
