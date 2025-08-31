using StoreSphere.IdentityAccess.Domain.common;
using StoreSphere.IdentityAccess.Domain.Events.Role;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Domain.Aggregates
{
    public class Role : AggregateRoot<RoleId>
    {
        private readonly List<PermissionId> _permissions = new();
        public ReadOnlyCollection<PermissionId> Permissions => _permissions.AsReadOnly();

        public string Name { get; private set; }

        private Role() { }

        public Role(RoleId id, string name)
        {
            Id = id;
            Name = name;

            AddDomainEvent(new RoleCreated(Id, Name));
        }

        public void AddPermission(PermissionId permissionId)
        {
            if (!_permissions.Contains(permissionId))
            {
                _permissions.Add(permissionId);
                AddDomainEvent(new PermissionAddedToRole(Id, permissionId));
            }
        }

        public void RemovePermission(PermissionId permissionId)
        {
            if (_permissions.Contains(permissionId)) { 
            _permissions.Remove(permissionId);
            AddDomainEvent(new PermissionRemovedFromRole(Id, permissionId));
            } 
        }
    }

}
