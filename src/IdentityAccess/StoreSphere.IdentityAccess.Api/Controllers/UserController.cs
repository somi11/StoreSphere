using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreSphere.IdentityAccess.Application.Features.Commands.Users.RegisterUser;
using StoreSphere.IdentityAccess.Application.Features.Commands.Users.RemoveUser;
using StoreSphere.IdentityAccess.Application.Features.Commands.Users.UpdateUser;

namespace StoreSphere.IdentityAccess.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // -------- Public: anyone can register --------
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken ct)
        {
            var userId = await _mediator.Send(command, ct);

            if (userId == Guid.Empty) // or null depending on your command result
            {
                return BadRequest(new { Message = "User registration failed." });
            }

            return Ok(new { UserId = userId });
        }
        // -------- Protected: only logged-in users --------
        [HttpPut("update")]
        [Authorize] // Any logged-in user; you can refine with policies
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand command, CancellationToken ct)
        {
            await _mediator.Send(command, ct);
            return NoContent();
        }

        // -------- Protected: only Admins --------
        [HttpDelete("remove/{userId:guid}")]
        [Authorize(Roles = "AppAdmin")] // Only Admins can remove users
        public async Task<IActionResult> Remove(Guid userId, CancellationToken ct)
        {
            await _mediator.Send(new RemoveUserCommand(userId), ct);
            return NoContent();
        }

        // -------- Protected: logged-in users --------
        [HttpGet("{id:guid}")]
        [Authorize] // Any logged-in user
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            // TODO: Add check: if current user == id OR user is Admin
            return Ok();
        }

        // Optional: cleaner way for self-profile
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe(CancellationToken ct)
        {
            // Use User.Claims to get current IdentityId/UserId
            return Ok();
        }
    }
}
