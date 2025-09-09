using Microsoft.AspNetCore.Identity;
using StoreSphere.IdentityAccess.Application.Contracts;
using StoreSphere.IdentityAccess.Infrastructure.Persistence;

namespace StoreSphere.IdentityAccess.Infrastructure.Authentication
{
    public class IdentityUserService : IIdentityUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly DomainDbContext _domainDbContext;

        public IdentityUserService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            AppIdentityDbContext identityDbContext,
            DomainDbContext domainDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _identityDbContext = identityDbContext;
            _domainDbContext = domainDbContext;
        }

        private static IdentityUserDto ToDto(IdentityUser user) =>
            new IdentityUserDto(user.Id, user.Email!);

        private static SignInResultDto ToDto(SignInResult result) =>
            new SignInResultDto(result.Succeeded, result.IsLockedOut, result.RequiresTwoFactor);

        // ---------------- User Management ----------------
        public async Task<IdentityUserDto?> CreateUserAsync(string email, string password,
            Dictionary<string, string>? claims = null)
        {
            if (await _userManager.FindByEmailAsync(email) != null)
                return null;

            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded) return null;

            if (claims != null && claims.Any())
            {
                foreach (var kvp in claims)
                {
                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(kvp.Key, kvp.Value));
                }
            }

            return ToDto(user);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<IdentityUserDto?> FindByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user == null ? null : ToDto(user);
        }

        public async Task<IdentityUserDto?> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user == null ? null : ToDto(user);
        }

        public async Task<bool> UpdateEmailAsync(string userId, string newEmail)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            var result = await _userManager.ChangeEmailAsync(user, newEmail, token);
            return result.Succeeded;
        }

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> ResetPasswordAsync(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }

        // ---------------- Role Management ----------------
        public async Task<bool> CreateRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName)) return true;
            var result = await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
            return result.Succeeded;
        }

        public async Task<bool> AssignRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> RemoveRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<IList<string>> GetRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user == null ? new List<string>() : await _userManager.GetRolesAsync(user);
        }

        public async Task<List<string>> GetUsersInRoleAsync(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return users.Select(u => u.Id).ToList();
        }

        // ---------------- Authentication ----------------
        public async Task<bool> CheckPasswordAsync(string userId, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null && await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<SignInResultDto> PasswordSignInAsync(string email, string password, bool isPersistent)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent, lockoutOnFailure: false);
            return ToDto(result);
        }

        // ---------------- Lockout & Security ----------------
        public async Task<bool> SetLockoutAsync(string userId, bool lockoutEnabled)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
            var result = await _userManager.SetLockoutEnabledAsync(user, lockoutEnabled);
            return result.Succeeded;
        }

        public async Task<bool> IsLockedOutAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null && await _userManager.IsLockedOutAsync(user);
        }

        public async Task<bool> UpdateClaimsAsync(string userId, Dictionary<string, string> claims)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var existingClaims = await _userManager.GetClaimsAsync(user);

            foreach (var kvp in claims)
            {
                var oldClaims = existingClaims.Where(c => c.Type == kvp.Key).ToList();
                if (oldClaims.Any())
                {
                    var removeResult = await _userManager.RemoveClaimsAsync(user, oldClaims);
                    if (!removeResult.Succeeded) return false;
                }

                var addResult = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(kvp.Key, kvp.Value));
                if (!addResult.Succeeded) return false;
            }

            return true;
        }
    }
}
