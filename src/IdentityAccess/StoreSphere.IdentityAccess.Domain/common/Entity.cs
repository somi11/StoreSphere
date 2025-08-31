using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Domain.common
{
    public abstract class Entity<TId>
    {
        public TId Id { get; protected set; }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity<TId> others) return false;
            if (ReferenceEquals(this, others)) return true;
            
            return Id!.Equals(others.Id);
        }

        public override int GetHashCode() {
            return Id!.GetHashCode();
        }
    }
}
