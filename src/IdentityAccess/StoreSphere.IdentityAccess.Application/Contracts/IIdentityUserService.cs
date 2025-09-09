
namespace StoreSphere.IdentityAccess.Application.Contracts
{
        public record IdentityUserDto(string Id, string Email);
        public record SignInResultDto(bool Succeeded, bool IsLockedOut, bool RequiresTwoFactor);

        public interface IIdentityUserService
        {
            // User management
            //Task<IdentityUserDto?> CreateUserAsync(string email, string password);
        Task<IdentityUserDto?> CreateUserAsync(string email, string password,
        Dictionary<string, string>? claims = null);
            Task<bool> DeleteUserAsync(string userId);
            Task<IdentityUserDto?> FindByIdAsync(string userId);
            Task<IdentityUserDto?> FindByEmailAsync(string email);
            Task<bool> UpdateEmailAsync(string userId, string newEmail);
            Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
            Task<List<string>> GetUsersInRoleAsync(string roleName);
            Task<bool> UpdateClaimsAsync(string userId, Dictionary<string, string> claims);
            Task<bool> ResetPasswordAsync(string userId, string newPassword);

        // Role management
        Task<bool> CreateRoleAsync(string roleName);
            Task<bool> AssignRoleAsync(string userId, string roleName);
            Task<bool> RemoveRoleAsync(string userId, string roleName);
            Task<IList<string>> GetRolesAsync(string userId);

            // Authentication
            Task<bool> CheckPasswordAsync(string userId, string password);
            Task<SignInResultDto> PasswordSignInAsync(string email, string password, bool isPersistent);

            // Lockout & Security
            Task<bool> SetLockoutAsync(string userId, bool lockoutEnabled);
            Task<bool> IsLockedOutAsync(string userId);
        }
    }
