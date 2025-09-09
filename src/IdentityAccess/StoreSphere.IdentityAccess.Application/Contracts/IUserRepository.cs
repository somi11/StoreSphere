using StoreSphere.IdentityAccess.Application.DTOs;
using StoreSphere.IdentityAccess.Domain.Aggregates;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.Contracts
{
    public interface IUserRepository
    {
        // Aggregate persistence
        Task Add(User user , string password);
        Task Update(User user, string? currentPassword, string? newPassword);
        Task Remove(User user);       // Optional: soft delete

        // Query methods for application / read layer
        Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);
        Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<UserDto>> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default);
        Task<List<UserDto>> GetByRoleAsync(string roleName, CancellationToken cancellationToken = default);

        // Optional: existence check
        Task<bool> ExistsAsync(UserId id, CancellationToken cancellationToken = default);
    }
}
