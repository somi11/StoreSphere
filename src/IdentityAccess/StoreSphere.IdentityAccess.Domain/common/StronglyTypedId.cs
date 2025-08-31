using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Domain.common
{
    public abstract record StronglyTypedId<T>(T Value)
    {
        public override string ToString() => Value!.ToString()!;
    }
}
