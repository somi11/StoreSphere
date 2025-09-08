using Microsoft.EntityFrameworkCore;
using StoreSphere.IdentityAccess.Application.Contracts;
using StoreSphere.IdentityAccess.Application.DTOs;
using StoreSphere.IdentityAccess.Domain.Aggregates;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;

namespace StoreSphere.IdentityAccess.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly DomainDbContext _dbContext;   // FIX: was IdentityAccessDbContext
        private readonly IIdentityUserService _identityUserService;

        public UserRepository(DomainDbContext dbContext, IIdentityUserService identityUserService)
        {
            _dbContext = dbContext;
            _identityUserService = identityUserService;
        }

        // --- Aggregate persistence ---
        public async Task Add(User user, string password)
        {
            // 1. Create identity user first
            var identityUser = await _identityUserService.CreateUserAsync(user.Email.Value, password);
            if (identityUser == null)
                throw new Exception("Failed to create identity user.");

            // 2. Map identity info to domain user
            user.IdentityId = identityUser.Id;

            // 3. Persist domain user
            _dbContext.Users.Add(user);
            // SaveChangesAsync is called by IUnitOfWork in handler
        }

        public async Task Update(User user, string? password)
        {
            // 1. Update identity user if needed
            if (!string.IsNullOrEmpty(password) && user.IdentityId != null)
            {
                var identityUser = await _identityUserService.FindByIdAsync(user.IdentityId);
                if (identityUser != null)
                {
                    // FIX: ChangePasswordAsync expects currentPassword, newPassword.
                    // If you don’t know current password, use RemovePassword + AddPassword instead.
                    await _identityUserService.ChangePasswordAsync(identityUser.Id, password, password);
                }
            }

            // 2. Update domain user
            _dbContext.Users.Update(user);
        }

        public async Task Remove(User user)
        {
            _dbContext.Users.Remove(user);
            // Optional: also remove from identity service
            if (!string.IsNullOrEmpty(user.IdentityId))
            {
                await _identityUserService.DeleteUserAsync(user.IdentityId);
            }
        }

        // --- Queries ---
        public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id.Value == id, cancellationToken);

            return user == null ? null : new UserDto(
                user.Id.Value,
                user.Email.Value,
                user.UserType.ToString(),
                user.TenantId?.Value,
                user.IsActive
            );
        }

        public async Task<List<UserDto>> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .Where(u => u.TenantId != null && u.TenantId.Value == tenantId)
                .Select(u => new UserDto(
                    u.Id.Value,
                    u.Email.Value,
                    u.UserType.ToString(),
                    u.TenantId != null ? u.TenantId.Value : (Guid?)null,
                    u.IsActive
                ))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<UserDto>> GetByRoleAsync(string roleName, CancellationToken cancellationToken = default)
        {
            var userIds = await _identityUserService.GetUsersInRoleAsync(roleName);

            if (userIds == null || userIds.Count == 0)
                return new List<UserDto>();

            return await _dbContext.Users
                .Where(u => u.IdentityId != null && userIds.Contains(u.IdentityId))
                .Select(u => new UserDto(
                    u.Id.Value,
                    u.Email.Value,
                    u.UserType.ToString(),
                    u.TenantId != null ? u.TenantId.Value : (Guid?)null,
                    u.IsActive
                ))
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(UserId id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.AnyAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<List<string>> GetRolesByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null || string.IsNullOrEmpty(user.IdentityId))
                return new List<string>();

            var identityUser = await _identityUserService.FindByIdAsync(user.IdentityId);
            if (identityUser == null)
                return new List<string>();

            return (await _identityUserService.GetRolesAsync(identityUser.Id)).ToList();
        }
    }
}
