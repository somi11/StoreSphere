using StoreSphere.IdentityAccess.Domain.Aggregates;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.Contracts
{
    public interface IStoreRepository
    {
        Task AddAsync(Store store, CancellationToken ct = default);
        Task<Store?> GetByIdAsync(StoreId id, CancellationToken ct = default);

        
        Task<IEnumerable<Store>> ListByTenantIdAsync(TenantId tenantId, CancellationToken ct = default);
        Task UpdateAsync(Store store, CancellationToken ct = default);


        // Added for user assignments
        Task<IEnumerable<Store>> GetByUserIdAsync(UserId userId, CancellationToken ct = default);
    }
}
