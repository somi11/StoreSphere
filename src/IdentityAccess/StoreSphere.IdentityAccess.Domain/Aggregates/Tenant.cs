using StoreSphere.IdentityAccess.Domain.common;
using StoreSphere.IdentityAccess.Domain.Entities;
using StoreSphere.IdentityAccess.Domain.Events.Tenant;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Domain.Aggregates
{
    public class Tenant : AggregateRoot<TenantId>
    {
        private readonly List<StoreId> _stores = new();
        public IReadOnlyCollection<StoreId> Stores => _stores.AsReadOnly();

        private readonly List<UserId> _users = new();
        public IReadOnlyCollection<UserId> Users => _users.AsReadOnly();

        public string Name { get; private set; }
        public UserId OwnerId { get; private set; }
        public TenantStatus Status { get; private set; }

        private Tenant() { }

        public Tenant(TenantId id, string name, UserId ownerId)
        {
            Id = id;
            Name = name;
            OwnerId = ownerId;
            Status = TenantStatus.Pending;

            AddDomainEvent(new TenantRegistered(Id, Name, OwnerId));
        }

        public void Activate()
        {
            Status = TenantStatus.Active;
            AddDomainEvent(new TenantActivated(Id));
        }

        public void Deactivate()
        {
            Status = TenantStatus.Suspended;
            AddDomainEvent(new TenantDeactivated(Id));
        }

        public void AddStore(StoreId storeId, string storeName)
        {
            _stores.Add(storeId);
            AddDomainEvent(new StoreAddedToTenant(Id, storeId, storeName));
        }

        public void RemoveStore(StoreId storeId)
        {
            if (_stores.Remove(storeId))
                AddDomainEvent(new StoreRemovedFromTenant(Id, storeId));
        }

        public void AddUser(UserId userId)
        {
            if (!_users.Contains(userId))
            {
                _users.Add(userId);
                AddDomainEvent(new UserAssignedToTenant(Id, userId));
            }
        }

        public void RemoveUser(UserId userId)
        {
            if (_users.Remove(userId))
                AddDomainEvent(new UserRemovedFromTenant(Id, userId));
        }
    }
}
