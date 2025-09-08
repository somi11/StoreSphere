using Microsoft.AspNetCore.Mvc;
using StoreSphere.IdentityAccess.Application.Contracts;

namespace StoreSphere.IdentityAccess.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityUserService _identityUserService;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(
            IIdentityUserService identityUserService,
            IJwtTokenService jwtTokenService)
        {
            _identityUserService = identityUserService;
            _jwtTokenService = jwtTokenService;
        }

        /// <summary>
        /// Authenticate with email + password and receive a JWT token.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // 1. Find user
            var user = await _identityUserService.FindByEmailAsync(request.Email);
            if (user == null)
                return Unauthorized("Invalid credentials.");

            // 2. Check password
            var validPassword = await _identityUserService.CheckPasswordAsync(user.Id, request.Password);
            if (!validPassword)
                return Unauthorized("Invalid credentials.");

            // 3. Generate JWT
            var token = await _jwtTokenService.GenerateTokenAsync(user.Id);

            return Ok(new LoginResponse
            {
                Token = token,
                ExpiresIn = 3600 // optional, could be taken from JwtSettings
            });
        }

        /// <summary>
        /// Refresh token endpoint (optional).
        /// Requires you to implement refresh token persistence.
        /// </summary>
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            // NOTE: Implement refresh token validation and rotation
            // For now, stubbed
            return Unauthorized("Refresh token flow not implemented yet.");
        }

        /// <summary>
        /// Logout endpoint (optional).
        /// Typically deletes refresh token server-side.
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // If using refresh tokens: delete them here.
            // If using cookie auth: call SignInManager.SignOutAsync().
            return Ok(new { Message = "Logged out." });
        }
    }

    // ---------------- DTOs ----------------
    public class LoginRequest
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = default!;
        public int ExpiresIn { get; set; }
    }

    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = default!;
    }
}
