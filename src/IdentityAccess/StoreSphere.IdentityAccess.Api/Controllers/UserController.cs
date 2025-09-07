using MediatR;
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken ct)
        {
            var userId = await _mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id = userId }, null);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand command, CancellationToken ct)
        {
            await _mediator.Send(command, ct);
            return NoContent();
        }

        [HttpDelete("remove/{userId:guid}")]
        public async Task<IActionResult> Remove(Guid userId, CancellationToken ct)
        {
            await _mediator.Send(new RemoveUserCommand(userId), ct);
            return NoContent();
        }

        // Example for GetById (for CreatedAtAction)
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            // Implementation omitted for brevity
            return Ok();
        }
    }
}