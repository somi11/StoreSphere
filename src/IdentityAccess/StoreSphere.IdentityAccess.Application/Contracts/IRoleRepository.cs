using StoreSphere.IdentityAccess.Domain.Aggregates;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.Contracts
{
    public interface IRoleRepository
    {
        Task AddAsync(Role role, CancellationToken ct = default);
        Task<Role?> GetByIdAsync(RoleId id, CancellationToken ct = default);
        Task<IEnumerable<Role>> ListAllAsync(CancellationToken ct = default);
        Task UpdateAsync(Role role, CancellationToken ct = default);
    }
}
