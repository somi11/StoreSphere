using Microsoft.AspNetCore.Identity;
using StoreSphere.IdentityAccess.Application.Contracts;
using StoreSphere.IdentityAccess.Infrastructure.Persistence;

namespace StoreSphere.IdentityAccess.Infrastructure.Authentication
{
    public class IdentityUserService : IIdentityUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly DomainDbContext _domainDbContext; // optional: bridge domain + identity

        public IdentityUserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            AppIdentityDbContext identityDbContext,
            DomainDbContext domainDbContext // optional
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _identityDbContext = identityDbContext;
            _domainDbContext = domainDbContext;
        }

        // ---------------- Helpers ----------------
        private static IdentityUserDto ToDto(ApplicationUser user) =>
            new IdentityUserDto(user.Id, user.Email!);

        private static SignInResultDto ToDto(SignInResult result) =>
            new SignInResultDto(result.Succeeded, result.IsLockedOut, result.RequiresTwoFactor);

        // ---------------- User Management ----------------
        public async Task<IdentityUserDto?> CreateUserAsync(string email, string password)
        {
            if (await _userManager.FindByEmailAsync(email) != null)
                return null;

            var user = new ApplicationUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // OPTIONAL: sync with domain aggregate
                // var domainUser = new User(new UserId(Guid.NewGuid()), UserType.Customer, Email.Create(email));
                // _domainDbContext.Users.Add(domainUser);
                // await _domainDbContext.SaveChangesAsync();

                return ToDto(user);
            }

            return null;
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

        // ---------------- Role Management ----------------
        public async Task<bool> CreateRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName)) return true;
            var result = await _roleManager.CreateAsync(new ApplicationRole { Name = roleName });
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
    }
}
