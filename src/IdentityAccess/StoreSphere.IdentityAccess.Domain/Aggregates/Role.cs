using StoreSphere.IdentityAccess.Domain.common;
using StoreSphere.IdentityAccess.Domain.Entities;
using StoreSphere.IdentityAccess.Domain.Events.Role;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using StoreSphere.IdentityAccess.Domain.ValueObjects.RoleScope;


namespace StoreSphere.IdentityAccess.Domain.Aggregates
{
    public class Role : AggregateRoot<RoleId>
    {
        public RoleScope Scope {  get; private set; }

        public string Name { get; private set; }
        // EF Core navigation for many-to-many Role → Permission
        private readonly List<RolePermission> _rolePermissions = new();
        public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

        private Role() { }

        public Role(RoleId id, string name , RoleScope scope )
        {
            Id = id;
            Name = name;
            Scope = scope;
            AddDomainEvent(new RoleCreated(Id, Name));
        }
    }

}
