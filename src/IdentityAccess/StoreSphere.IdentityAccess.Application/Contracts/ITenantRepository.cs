using StoreSphere.IdentityAccess.Domain.Aggregates;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.Contracts
{
   public interface ITenantRepository
    {
        Task AddAsync(Tenant tenant, CancellationToken ct = default);
        Task<Tenant?> GetByIdAsync(TenantId id, CancellationToken ct = default);
        Task UpdateAsync(Tenant tenant, CancellationToken ct = default);
    }
}
