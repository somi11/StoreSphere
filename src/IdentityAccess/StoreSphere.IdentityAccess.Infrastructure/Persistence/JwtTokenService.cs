using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StoreSphere.IdentityAccess.Application.Contracts;
using StoreSphere.IdentityAccess.Infrastructure.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StoreSphere.IdentityAccess.Infrastructure.Authentication
{
    /// <summary>
    /// Service for generating JWT tokens enriched with Identity and Domain claims.
    /// </summary>
    public class JwtTokenService : IJwtTokenService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DomainDbContext _domainDbContext;
        private readonly JwtSettings _jwtSettings;

        public JwtTokenService(
            UserManager<IdentityUser> userManager,
            DomainDbContext domainDbContext,
            IOptions<JwtSettings> jwtOptions)
        {
            _userManager = userManager;
            _domainDbContext = domainDbContext;
            _jwtSettings = jwtOptions.Value;
        }

        /// <summary>
        /// Generates a JWT token for a given user by combining:
        /// - ASP.NET Identity roles
        /// - Domain global roles and permissions
        /// - Domain store roles and permissions
        /// </summary>
        public async Task<string> GenerateTokenAsync(string userId)
        {
            // 🔹 1. Load Identity user
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            // ASP.NET Identity roles
            var identityRoles = await _userManager.GetRolesAsync(user);

            // 🔹 2. Load Domain user
            var domainUser = await _domainDbContext.Users
                .Include(u => u.RoleAssignments)
                .FirstOrDefaultAsync(u => u.IdentityId == user.Id);

            if (domainUser == null)
                throw new InvalidOperationException("Domain user not found for Identity user");

            // 🔹 3. Load global roles & permissions
            var roleIds = domainUser.RoleAssignments
                .Select(ra => ra.RoleId)
                .ToList();

            var roles = await _domainDbContext.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .Where(r => roleIds.Contains(r.Id))
                .ToListAsync();

            var domainRoles = roles.Select(r => r.Name).ToList();

            var permissions = roles
                .SelectMany(r => r.RolePermissions.Select(rp => rp.Permission.Name))
                .Distinct()
                .ToList();

            // 🔹 4. Load store roles & permissions
            var storeAssignments = await _domainDbContext.StoreUserAssignments
                .Where(sua => sua.UserId == domainUser.Id)
                .ToListAsync();

            var storeRoleIds = storeAssignments.Select(sua => sua.RoleId).ToList();

            var storeRoles = await _domainDbContext.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .Where(r => storeRoleIds.Contains(r.Id))
                .ToListAsync();

            // (Optional) Store roles grouped by Store
            var userStoreRoles = storeAssignments.Select(a => new
            {
                StoreId = a.StoreId.Value,
                Role = storeRoles.First(r => r.Id == a.RoleId).Name
            }).ToList();

            var storePermissions = storeRoles
                .SelectMany(r => r.RolePermissions.Select(rp => rp.Permission.Name))
                .Distinct()
                .ToList();

            // 🔹 5. Build claims
            var claims = new List<Claim>
            {
                // Standard Identity claims
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),

                // Domain user identity
                new Claim("domainUserId", domainUser.Id.Value.ToString()),

                // Domain-specific attributes
                new Claim("userType", domainUser.UserType.ToString()),
                new Claim("isActive", domainUser.IsActive.ToString().ToLowerInvariant())
            };

            if (domainUser.TenantId is not null)
            {
                claims.Add(new Claim("tenantId", domainUser.TenantId.Value.ToString()));
            }

            // Identity roles
            claims.AddRange(identityRoles.Select(r => new Claim(ClaimTypes.Role, r)));

            // Domain global roles
            claims.AddRange(domainRoles.Select(r => new Claim("domainRole", r)));

            // Domain global permissions
            claims.AddRange(permissions.Select(p => new Claim("permission", p)));

            // Store-scoped permissions (optional, might be large!)
            claims.AddRange(storePermissions.Select(p => new Claim("storePermission", p)));

            // (Optional) Add store-specific roles (as claims with storeId context)
            foreach (var assignment in userStoreRoles)
            {
                claims.Add(new Claim($"store:{assignment.StoreId}:role", assignment.Role));
            }

            // 🔹 6. Create JWT
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
