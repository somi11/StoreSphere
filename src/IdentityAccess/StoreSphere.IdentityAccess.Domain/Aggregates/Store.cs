using StoreSphere.IdentityAccess.Domain.common;
using StoreSphere.IdentityAccess.Domain.Entities;
using StoreSphere.IdentityAccess.Domain.Events.Store;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using StoreSphere.IdentityAccess.Domain.ValueObjects.StoreStatus;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Domain.Aggregates
{
    public class Store : AggregateRoot<StoreId>
    {
        private readonly List<StoreUserAssignment> _assignments = new();
        public ReadOnlyCollection<StoreUserAssignment> Assignments => _assignments.AsReadOnly();

        public TenantId TenantId { get; private set; }
        public string Name { get; private set; }
        public string? DomainName { get; private set; }
        public StoreStatus Status { get; private set; }

        private Store() { } // EF Core / serialization

        public Store(StoreId id, TenantId tenantId, string name, string? domainName = null)
        {
            Id = id;
            TenantId = tenantId;
            Name = name;
            DomainName = domainName;
            Status = StoreStatus.Active;

            AddDomainEvent(new StoreCreated(Id, TenantId, Name, DomainName));
        }

        // --- Lifecycle operations ---

        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Store name cannot be empty.", nameof(newName));

            if (Name != newName)
            {
                Name = newName;
                AddDomainEvent(new StoreRenamed(Id, newName));
            }
        }

        public void ChangeDomain(string? newDomainName)
        {
            if (DomainName != newDomainName)
            {
                DomainName = newDomainName;
                AddDomainEvent(new StoreDomainChanged(Id, newDomainName));
            }
        }

        public void Activate()
        {
            if (Status != StoreStatus.Active)
            {
                Status = StoreStatus.Active;
                AddDomainEvent(new StoreActivated(Id));
            }
        }

        public void Deactivate()
        {
            if (Status != StoreStatus.Inactive)
            {
                Status = StoreStatus.Inactive;
                AddDomainEvent(new StoreDeactivated(Id));
            }
        }

        // --- User assignments ---

        public void AssignUser(UserId userId, RoleId roleId)
        {
            if (_assignments.Any(a => a.UserId == userId && a.RoleId == roleId))
                return; // Already assigned, no duplicates

            var assignment = new StoreUserAssignment(
                new StoreUserAssignmentId(Guid.NewGuid()), Id, userId, roleId
            );

            _assignments.Add(assignment);

            AddDomainEvent(new UserAssignedToStore(Id, userId, roleId));
        }

        public void RemoveUser(UserId userId, RoleId roleId)
        {
            var assignment = _assignments
                .FirstOrDefault(a => a.UserId == userId && a.RoleId == roleId);

            if (assignment != null)
            {
                _assignments.Remove(assignment);
                AddDomainEvent(new UserRemovedFromStore(Id, userId, roleId));
            }
        }
    }

}
