using Microsoft.EntityFrameworkCore;
using StoreSphere.IdentityAccess.Application.Contracts;
using StoreSphere.IdentityAccess.Application.DTOs;
using StoreSphere.IdentityAccess.Domain.Aggregates;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityAccessDbContext _dbContext;
        private readonly IIdentityUserService _identityUserService;

        public UserRepository(IdentityAccessDbContext dbContext, IIdentityUserService identityUserService)
        {
            _dbContext = dbContext;
            _identityUserService = identityUserService;
        }
        // Aggregate persistence
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
                    await _identityUserService.ChangePasswordAsync(identityUser.Id, password, password);
                }
            }
            // 2. Update domain user
            _dbContext.Users.Update(user);
            // SaveChangesAsync is called by IUnitOfWork in handler
        }

        public async Task Remove(User user)
        {
            // 1. Remove domain user
            _dbContext.Users.Remove(user);
            // 2. Remove identity user (async, so consider doing this in handler if needed)
            // SaveChangesAsync is called by IUnitOfWork in handler
        }
        // Query methods
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

            // Ensure userIds is not null
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
            // Find the domain user first
            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null || string.IsNullOrEmpty(user.IdentityId))
                return new List<string>();

            // Use IdentityId (string) to find the identity user
            var identityUser = await _identityUserService.FindByIdAsync(user.IdentityId);
            if (identityUser == null)
                return new List<string>();

            return (await _identityUserService.GetRolesAsync(identityUser.Id)).ToList();

        }

    }
}
