using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.Features.Commands.Users.RegisterUser
{
    public record RegisterUserCommand(string Email, string UserType, string Password, Guid? TenantId) : IRequest<Guid>;

}
