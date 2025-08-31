using StoreSphere.IdentityAccess.Domain.common;
using StoreSphere.IdentityAccess.Domain.Events.User;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Accounts;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using StoreSphere.IdentityAccess.Domain.ValueObjects.UserType;
using System.Collections.ObjectModel;


namespace StoreSphere.IdentityAccess.Domain.Aggregates
{
    public class User : AggregateRoot<UserId>
    {
        private readonly List<Role> _roles = new();
        public ReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

        public TenantId? TenantId { get; private set; }   // only for Merchant users
        public Email Email { get; private set; }
        public bool IsActive { get; private set; }
        public UserType UserType { get; private set; }

        private User() { } // EF Core / ORM constructor

        public User(UserId id, UserType userType, Email email, TenantId? tenantId = null)
        {
            // Business rule: Only Merchant can have a TenantId
            if (userType == UserType.Merchant && tenantId is null)
                throw new InvalidOperationException("Merchant users must have a TenantId.");

            if (userType != UserType.Merchant && tenantId is not null)
                throw new InvalidOperationException("Only Merchant users can have a TenantId.");

            Id = id;
            UserType = userType;
            Email = email;
            TenantId = tenantId;
            IsActive = true;

            AddDomainEvent(new UserRegistered(Id, UserType, Email, TenantId));
        }

        public void Activate()
        {
            if (!IsActive)
            {
                IsActive = true;
                AddDomainEvent(new UserActivated(Id));
            }
        }

        public void Deactivate()
        {
            if (IsActive)
            {
                IsActive = false;
                AddDomainEvent(new UserDeactivated(Id));
            }
        }

        public void AddRole(Role role)
        {
            if (!_roles.Any(r => r.Id == role.Id))
            {
                _roles.Add(role);
                AddDomainEvent(new RoleAssignedToUser(Id, role.Id));
            }
        }

        public void RemoveRole(Role role)
        {
            var existingRole = _roles.FirstOrDefault(r => r.Id == role.Id);
            if (existingRole != null)
            {
                _roles.Remove(existingRole);
                AddDomainEvent(new RoleRemovedFromUser(Id, role.Id));
            }
        }

        public void ChangeEmail(Email newEmail)
        {
            if (newEmail != Email)
            {
                Email = newEmail;
                AddDomainEvent(new UserEmailChanged(Id, newEmail));
            }
        }
        public void ChangeUserType(UserType newUserType)
        {
            if (UserType == newUserType)
                return; 

            // Business rules: Only Merchant users can have TenantId
            if (newUserType == UserType.Merchant && TenantId is null)
                throw new InvalidOperationException("Merchant users must have a TenantId.");

            if (newUserType != UserType.Merchant && TenantId is not null)
                TenantId = null; // clear tenant if changing to non-merchant

            UserType = newUserType;

            // Raise domain event
            AddDomainEvent(new UserTypeChanged(Id, newUserType));
        }
    }
}
