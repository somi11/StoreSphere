using StoreShpere.SharedKernel.Events;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Accounts;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using StoreSphere.IdentityAccess.Domain.ValueObjects.UserType;


namespace StoreSphere.IdentityAccess.Domain.Events.User
{

    public record UserTypeChanged(UserId UserId, UserType UserType) : IDomainEvent;

}
