using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.Contracts
{
    public interface IIdentityUserService
    {
        // User management
        Task<IdentityUser?> CreateUserAsync(string email, string password);
        Task<bool> DeleteUserAsync(string userId);
        Task<IdentityUser?> FindByIdAsync(string userId);
        Task<IdentityUser?> FindByEmailAsync(string email);
        Task<bool> UpdateEmailAsync(string userId, string newEmail);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<List<string>> GetUsersInRoleAsync(string roleName);


        // Role management
        Task<bool> CreateRoleAsync(string roleName);
        Task<bool> AssignRoleAsync(string userId, string roleName);
        Task<bool> RemoveRoleAsync(string userId, string roleName);
        Task<IList<string>> GetRolesAsync(string userId);

        // Authentication
        Task<bool> CheckPasswordAsync(string userId, string password);
        Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent);

        // Lockout & Security
        Task<bool> SetLockoutAsync(string userId, bool lockoutEnabled);
        Task<bool> IsLockedOutAsync(string userId);
    }


}
